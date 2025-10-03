using System;
using System.Collections.Generic;
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
                foreach (ListItem item in chkCourses.Items)
                {
                    if (item.Selected)
                        selectedCourses.Add(Convert.ToInt32(item.Value));
                }

                Teacher teacher = new Teacher
                {
                    TeacherName = txtTeacherName.Text,
                    CourseIds = selectedCourses
                };

                teacherData.AddTeacher(teacher);
                txtTeacherName.Text = "";
                foreach (ListItem item in chkCourses.Items) item.Selected = false;

                LoadTeachers();
                lblMessage.Text = "Teacher added successfully!";
            }
        }

        protected void gvTeachers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int teacherId = Convert.ToInt32(gvTeachers.DataKeys[e.RowIndex].Value);
            teacherData.DeleteTeacher(teacherId);
            LoadTeachers();
            lblMessage.Text = "Teacher deleted successfully!";
        }

        protected void gvTeachers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTeachers.EditIndex = e.NewEditIndex;
            LoadTeachers();

            GridViewRow row = gvTeachers.Rows[e.NewEditIndex];
            CheckBoxList chkEditCourses = (CheckBoxList)row.FindControl("chkEditCourses");

            if (chkEditCourses != null)
            {
                // Load all courses
                chkEditCourses.DataSource = courseData.GetAllCourses();
                chkEditCourses.DataTextField = "CourseName";
                chkEditCourses.DataValueField = "CourseId";
                chkEditCourses.DataBind();

                // Pre-select current teacher's courses
                int teacherId = Convert.ToInt32(gvTeachers.DataKeys[e.NewEditIndex].Value);
                Teacher teacher = teacherData.GetTeacherById(teacherId);

                if (teacher.CourseIds != null)
                {
                    foreach (ListItem item in chkEditCourses.Items)
                    {
                        if (teacher.CourseIds.Contains(Convert.ToInt32(item.Value)))
                            item.Selected = true;
                    }
                }
            }
        }

        protected void gvTeachers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTeachers.EditIndex = -1;
            LoadTeachers();
        }

        protected void gvTeachers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int teacherId = Convert.ToInt32(gvTeachers.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvTeachers.Rows[e.RowIndex];

            // Get updated name
            TextBox txtTeacherName = (TextBox)row.FindControl("txtEditTeacherName");
            string teacherName = txtTeacherName.Text;

            // Get selected courses
            CheckBoxList chkEditCourses = (CheckBoxList)row.FindControl("chkEditCourses");
            List<int> selectedCourses = new List<int>();
            foreach (ListItem item in chkEditCourses.Items)
            {
                if (item.Selected)
                    selectedCourses.Add(Convert.ToInt32(item.Value));
            }

            Teacher updatedTeacher = new Teacher
            {
                TeacherId = teacherId,
                TeacherName = teacherName,
                CourseIds = selectedCourses
            };

            teacherData.UpdateTeacher(updatedTeacher);

            gvTeachers.EditIndex = -1;
            LoadTeachers();
            lblMessage.Text = "Teacher updated successfully!";
        }
    }
}
