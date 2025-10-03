using System;
using System.Collections.Generic;
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
    }
}
