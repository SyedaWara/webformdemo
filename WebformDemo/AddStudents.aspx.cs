using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebformDemo.Models;

namespace WebformDemo
{
    public partial class AddStudents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        private void LoadCourses()
        {
            string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                string query = "SELECT CourseID, CourseName FROM Courses";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                chkCourses.DataSource = reader;
                chkCourses.DataTextField = "CourseName";
                chkCourses.DataValueField = "CourseID";
                chkCourses.DataBind();
            }
        }
        private void LoadTeachersForCourses()
        {
            var selectedCourses = chkCourses.Items.Cast<ListItem>()
                                       .Where(i => i.Selected)
                                       .Select(i => new { CourseID = i.Value, CourseName = i.Text })
                                       .ToList();

            List<CourseTeacher> courseTeacherList = new List<CourseTeacher>();
            string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                foreach (var course in selectedCourses)
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT t.TeacherID, t.TeacherName 
                                              FROM TeacherCourses tc
                                              INNER JOIN Teachers t ON tc.TeacherID = t.TeacherID
                                              WHERE tc.CourseID=@CourseID", con);
                    cmd.Parameters.AddWithValue("@CourseID", course.CourseID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        courseTeacherList.Add(new CourseTeacher
                        {
                            CourseID = int.Parse(course.CourseID),
                            CourseName = course.CourseName,
                            TeacherID = (int)reader["TeacherID"],
                            TeacherName = reader["TeacherName"].ToString()
                        });
                    }
                    reader.Close();
                }
            }

            // Bind grouped data to Repeater
            rptCourseTeachers.DataSource = courseTeacherList.GroupBy(ct => new { ct.CourseID, ct.CourseName })
                .Select(g => new
                {
                    CourseID = g.Key.CourseID,
                    CourseName = g.Key.CourseName,
                    Teachers = g.Select(t => new { t.TeacherID, t.TeacherName }).ToList()
                }).ToList();
            rptCourseTeachers.DataBind();
        }


        protected void rptCourseTeachers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var hfCourseID = (HiddenField)e.Item.FindControl("hfCourseID");
            hfCourseID.Value = DataBinder.Eval(e.Item.DataItem, "CourseID").ToString();

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var ddl = (DropDownList)e.Item.FindControl("ddlTeachers");
                

                // Teachers bind
                var teachers = DataBinder.Eval(e.Item.DataItem, "Teachers") as IEnumerable<object>;
                if (teachers != null)
                {
                    ddl.DataSource = teachers;
                    ddl.DataTextField = "TeacherName";
                    ddl.DataValueField = "TeacherID";
                    ddl.DataBind();
                }
                ddl.Items.Insert(0, new ListItem("--Select Teacher--", "0"));

                // CourseID bind hidden field
                hfCourseID.Value = DataBinder.Eval(e.Item.DataItem, "CourseID").ToString();
            }
        }




        protected void btnAdd_Click(object sender, EventArgs e)
        {
            

            if (!TxtEmail.Text.Contains("@"))
            {
                Response.Write("<script>alert('Invalid Email. Email must contain @');</script>");
                return;
            }

            

            string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Insert Student
                    string insertStudent = @"INSERT INTO Students (Name, RollNo, Class, Section, Email)
                                     OUTPUT INSERTED.StudentID
                                     VALUES (@Name,@RollNo,@Class,@Section,@Email)";
                    SqlCommand insertCmd = new SqlCommand(insertStudent, con, transaction);
                    insertCmd.Parameters.AddWithValue("@Name", TxtName.Text);
                    insertCmd.Parameters.AddWithValue("@RollNo", TxtRollNo.Text);
                    insertCmd.Parameters.AddWithValue("@Class", TxtClass.Text);
                    insertCmd.Parameters.AddWithValue("@Section", TxtSection.Text);
                    insertCmd.Parameters.AddWithValue("@Email", TxtEmail.Text);

                    int studentId = (int)insertCmd.ExecuteScalar();

                    // Insert Student-Course-Teacher mapping
                    foreach (RepeaterItem item in rptCourseTeachers.Items)
                    {
                        var ddlTeachers = (DropDownList)item.FindControl("ddlTeachers");
                        var hfCourseID = (HiddenField)item.FindControl("hfCourseID");

                        if (hfCourseID == null || string.IsNullOrEmpty(hfCourseID.Value))
                        {
                            Response.Write("<script>alert('CourseID missing!');</script>");
                            continue;
                        }

                        int courseId = Convert.ToInt32(hfCourseID.Value);

                        if (ddlTeachers.SelectedValue != "0")
                        {
                            int teacherId = Convert.ToInt32(ddlTeachers.SelectedValue);

                            Response.Write("<script>alert('Inserting: StudentID=" + studentId +
                                           ", CourseID=" + courseId +
                                           ", TeacherID=" + teacherId + "');</script>");

                            string insertStudentCourse = @"INSERT INTO StudentCourses(StudentID, CourseID, TeacherID) 
                                       VALUES(@StudentID,@CourseID,@TeacherID)";
                            SqlCommand cmd = new SqlCommand(insertStudentCourse, con, transaction);
                            cmd.Parameters.AddWithValue("@StudentID", studentId);
                            cmd.Parameters.AddWithValue("@CourseID", courseId);
                            cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                            cmd.ExecuteNonQuery();
                        }
                    }


                    transaction.Commit();
                    Response.Write("<script>alert('Student added successfully with courses!');</script>");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Response.Write("<script>alert('Error adding student: " + ex.Message + "');</script>");
                }
            }
        }


       
        protected void chkCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTeachersForCourses();
        }

    }
}
