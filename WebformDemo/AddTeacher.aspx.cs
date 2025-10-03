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
                BindTeachersGrid();
            }
        }

        private void LoadCourses()
        {
            chkCourses.DataSource = courseData.GetAllCourses();
            chkCourses.DataTextField = "CourseName";
            chkCourses.DataValueField = "CourseId";
            chkCourses.DataBind();
        }

        private void BindTeachersGrid()
        {
            gvTeachers.DataSource = teacherData.GetAllTeachers();
            gvTeachers.DataBind();
        }

        protected void btnAddTeacher_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTeacherName.Text))
            {
                Teacher teacher = new Teacher
                {
                    TeacherName = txtTeacherName.Text,
                    CourseIds = GetSelectedCourseIds(chkCourses)
                };

                teacherData.AddTeacher(teacher);

                txtTeacherName.Text = "";
                foreach (ListItem item in chkCourses.Items) item.Selected = false;

                BindTeachersGrid();
                lblMessage.Text = "Teacher added successfully!";
            }
        }

        private List<int> GetSelectedCourseIds(CheckBoxList chkList)
        {
            List<int> ids = new List<int>();
            foreach (ListItem item in chkList.Items)
            {
                if (item.Selected)
                    ids.Add(Convert.ToInt32(item.Value));
            }
            return ids;
        }

        // ---------------- GridView Events ----------------

        protected void gvTeachers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTeachers.EditIndex = e.NewEditIndex;
            BindTeachersGrid();
        }

        protected void gvTeachers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTeachers.EditIndex = -1;
            BindTeachersGrid();
        }

        protected void gvTeachers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvTeachers.EditIndex == e.Row.RowIndex)
            {
                int teacherId = Convert.ToInt32(gvTeachers.DataKeys[e.Row.RowIndex].Value);
                Teacher teacher = teacherData.GetTeacherById(teacherId);

                CheckBoxList chkEditCourses = (CheckBoxList)e.Row.FindControl("chkEditCourses");
                if (chkEditCourses != null)
                {
                    chkEditCourses.DataSource = courseData.GetAllCourses();
                    chkEditCourses.DataTextField = "CourseName";
                    chkEditCourses.DataValueField = "CourseId";
                    chkEditCourses.DataBind();

                    // Select teacher's current courses
                    foreach (ListItem item in chkEditCourses.Items)
                    {
                        if (teacher.CourseIds != null && teacher.CourseIds.Contains(Convert.ToInt32(item.Value)))
                            item.Selected = true;
                    }
                }
            }
        }

        protected void gvTeachers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvTeachers.Rows[e.RowIndex];
            int teacherId = Convert.ToInt32(gvTeachers.DataKeys[e.RowIndex].Value);

            // Update Name
            TextBox txtName = (TextBox)row.FindControl("txtTeacherName");
            string teacherName = txtName.Text;

            // Update selected courses
            CheckBoxList chkCourses = (CheckBoxList)row.FindControl("chkEditCourses");
            List<int> selectedCourses = new List<int>();
            foreach (ListItem item in chkCourses.Items)
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
            BindTeachersGrid();
            lblMessage.Text = "Teacher updated successfully!";
        }

        protected void gvTeachers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int teacherId = Convert.ToInt32(gvTeachers.DataKeys[e.RowIndex].Value);
            teacherData.DeleteTeacher(teacherId);
            BindTeachersGrid();
            lblMessage.Text = "Teacher deleted successfully!";
        }
    }
}
