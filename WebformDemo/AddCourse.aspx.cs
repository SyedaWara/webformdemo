using System;
using WebformDemo.Models;

namespace WebformDemo
{
    public partial class AddCourse : System.Web.UI.Page
    {
        private CourseData courseData = new CourseData();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        protected void btnAddCourse_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCourseName.Text))
            {
                Course course = new Course
                {
                    CourseName = txtCourseName.Text
                };

                courseData.AddCourse(course);
                txtCourseName.Text = "";
                LoadCourses();
            }
        }

        private void LoadCourses()
        {
            gvCourses.DataSource = courseData.GetAllCourses();
            gvCourses.DataBind();
        }


        protected void gvCourses_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int courseId = Convert.ToInt32(gvCourses.DataKeys[e.RowIndex].Value);
            courseData.DeleteCourse(courseId);
            LoadCourses();
        }
    }
}
