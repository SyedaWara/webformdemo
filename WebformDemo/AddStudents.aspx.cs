using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using WebformDemo.Models;

namespace WebformDemo
{
    public partial class AddStudents : System.Web.UI.Page
    {
        // Model Binding for Courses
        public IEnumerable<Course> GetCourses()
        {
            List<Course> courses = new List<Course>();
            string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT CourseID, CourseName FROM Courses", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    courses.Add(new Course
                    {
                        CourseID = (int)reader["CourseID"],
                        CourseName = reader["CourseName"].ToString()
                    });
                }
            }

            return courses;
        }

        // Insert Student
        public void InsertStudent(Student student)
        {
            string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                bool transactionCompleted = false;

                try
                {
                    string insertStudent = @"INSERT INTO Students (Name, RollNo, Class, Section, Email)
                                             OUTPUT INSERTED.StudentID
                                             VALUES (@Name,@RollNo,@Class,@Section,@Email)";
                    SqlCommand cmd = new SqlCommand(insertStudent, con, transaction);
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@RollNo", student.RollNo);
                    cmd.Parameters.AddWithValue("@Class", student.Class);
                    cmd.Parameters.AddWithValue("@Section", student.Section);
                    cmd.Parameters.AddWithValue("@Email", student.Email);

                    int studentId = (int)cmd.ExecuteScalar();

                    // Student-Course-Teacher mapping
                    Repeater rptCourseTeachersInside = (Repeater)fvStudent.FindControl("rptCourseTeachers");
                    if (rptCourseTeachersInside != null)
                    {
                        foreach (RepeaterItem item in rptCourseTeachersInside.Items)
                        {
                            var hfCourseID = (HiddenField)item.FindControl("hfCourseID");
                            var ddl = (DropDownList)item.FindControl("ddlTeachers");

                            if (hfCourseID != null && ddl != null && ddl.SelectedValue != "0")
                            {
                                string insertMapping = @"INSERT INTO StudentCourses(StudentID, CourseID, TeacherID)
                                                         VALUES(@StudentID,@CourseID,@TeacherID)";
                                SqlCommand cmdMap = new SqlCommand(insertMapping, con, transaction);
                                cmdMap.Parameters.AddWithValue("@StudentID", studentId);
                                cmdMap.Parameters.AddWithValue("@CourseID", int.Parse(hfCourseID.Value));
                                cmdMap.Parameters.AddWithValue("@TeacherID", int.Parse(ddl.SelectedValue));
                                cmdMap.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                    transactionCompleted = true;
                }
                catch
                {
                    if (!transactionCompleted)
                        transaction.Rollback();
                    throw;
                }
            }
        }

        // FormView ItemInserted Event
        protected void fvStudent_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            lblSuccess.Text = "Student added successfully!";
            fvStudent.ChangeMode(FormViewMode.Insert); // resets form
        }

        // Repeater ItemDataBound for Teacher dropdown
        protected void rptCourseTeachers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var hfCourseID = (HiddenField)e.Item.FindControl("hfCourseID");
                var ddl = (DropDownList)e.Item.FindControl("ddlTeachers");

                if (hfCourseID != null && ddl != null)
                {
                    string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand(@"SELECT t.TeacherID, t.TeacherName 
                                                          FROM TeacherCourses tc
                                                          INNER JOIN Teachers t ON tc.TeacherID = t.TeacherID
                                                          WHERE tc.CourseID=@CourseID", con);
                        cmd.Parameters.AddWithValue("@CourseID", int.Parse(hfCourseID.Value));
                        SqlDataReader reader = cmd.ExecuteReader();

                        List<Teacher> teachers = new List<Teacher>();
                        while (reader.Read())
                        {
                            teachers.Add(new Teacher
                            {
                                TeacherId = (int)reader["TeacherID"],
                                TeacherName = reader["TeacherName"].ToString()
                            });
                        }

                        ddl.DataSource = teachers;
                        ddl.DataTextField = "TeacherName";
                        ddl.DataValueField = "TeacherId";
                        ddl.DataBind();
                        ddl.Items.Insert(0, new ListItem("--Select Teacher--", "0"));
                    }
                }
            }
        }

        // CheckBoxList SelectedIndexChanged
        protected void chkCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList chkCoursesInside = (CheckBoxList)fvStudent.FindControl("chkCourses");
            Repeater rptCourseTeachersInside = (Repeater)fvStudent.FindControl("rptCourseTeachers");

            if (chkCoursesInside == null || rptCourseTeachersInside == null)
                return;

            List<Course> selectedCourses = chkCoursesInside.Items.Cast<ListItem>()
                .Where(i => i.Selected)
                .Select(i => new Course { CourseID = int.Parse(i.Value), CourseName = i.Text })
                .ToList();

            rptCourseTeachersInside.DataSource = selectedCourses;
            rptCourseTeachersInside.DataBind();
        }
    }
}
