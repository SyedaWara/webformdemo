using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using WebformDemo.Models;

namespace WebformDemo
{
    public partial class AddCourse : System.Web.UI.Page
    {
        private CourseData courseData = new CourseData();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // Add new course
        protected void btnAddCourse_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCourseName.Text))
            {
                courseData.AddCourse(new Course
                {
                    CourseName = txtCourseName.Text
                });

                txtCourseName.Text = "";
                gvCourses.DataBind(); // refresh GridView
            }
        }

        // Model Binding Methods

        public List<Course> GetCourses()
        {
            return courseData.GetAllCourses();
        }

        public void UpdateCourse(int CourseID)
        {
            GridViewRow row = gvCourses.Rows[gvCourses.EditIndex];
            var txtName = (System.Web.UI.WebControls.TextBox)row.FindControl("txtCourseNameEdit");

            if (txtName != null)
            {
                courseData.UpdateCourse(new Course
                {
                    CourseID = CourseID,
                    CourseName = txtName.Text
                });
            }
        }

        public void DeleteCourse(int CourseID)
        {
            courseData.DeleteCourse(CourseID);
        }
    }
}
