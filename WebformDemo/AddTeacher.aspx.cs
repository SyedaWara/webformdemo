using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using WebformDemo.Models;

namespace WebformDemo
{
    public partial class AddTeacher : System.Web.UI.Page
    {
        private TeacherData teacherData = new TeacherData();
        private CourseData courseData = new CourseData();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCourses();
                LoadTeachers();
            }
        }
        public List<Teacher> GetAllTeachers()
        {
            string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;
            List<Teacher> teachers = new List<Teacher>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
            SELECT t.TeacherId, 
                   t.TeacherName, 
                   STRING_AGG(c.CourseName, ', ') AS Courses
            FROM Teachers t
            LEFT JOIN TeacherCourses tc ON t.TeacherId = tc.TeacherId
            LEFT JOIN Courses c ON tc.CourseId = c.CourseId
            GROUP BY t.TeacherId, t.TeacherName;
        ";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    teachers.Add(new Teacher
                    {
                        TeacherId = Convert.ToInt32(reader["TeacherId"]),
                        TeacherName = reader["TeacherName"].ToString(),
                        Courses = reader["Courses"] == DBNull.Value ? "" : reader["Courses"].ToString()
                    });
                }
            }
            return teachers;
        }

        private void LoadCourses()
        {
            chkCourses.DataSource = courseData.GetAllCourses();
            chkCourses.DataTextField = "CourseName";
            chkCourses.DataValueField = "CourseId";
            chkCourses.DataBind();
        }

        private void LoadTeachers()
        {
            gvTeachers.DataSource = teacherData.GetAllTeachers();
            gvTeachers.DataBind();
        }

        protected void btnAddTeacher_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTeacherName.Text))
            {
                List<int> selectedCourses = new List<int>();
                foreach (var item in chkCourses.Items)
                {
                    var chkItem = (System.Web.UI.WebControls.ListItem)item;
                    if (chkItem.Selected)
                    {
                        selectedCourses.Add(Convert.ToInt32(chkItem.Value));
                    }
                }

                Teacher teacher = new Teacher
                {
                    TeacherName = txtTeacherName.Text,
                    CourseIds = selectedCourses
                };

                teacherData.AddTeacher(teacher);
                txtTeacherName.Text = "";
                LoadTeachers();
            }
        }
        protected void gvTeachers_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int teacherId = Convert.ToInt32(gvTeachers.DataKeys[e.RowIndex].Value);

            TeacherData teacherData = new TeacherData();
            teacherData.DeleteTeacher(teacherId);

            LoadTeachers(); // refresh after delete
        }

    }
}
