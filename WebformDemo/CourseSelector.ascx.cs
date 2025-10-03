using System;
using System.Collections.Generic;
using System.Linq;
using WebformDemo.Models;

namespace WebformDemo.Controls
{
    public partial class CourseSelector : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SchoolData data = new SchoolData();
                chkCourses.DataSource = data.GetCourses();
                chkCourses.DataTextField = "CourseName";
                chkCourses.DataValueField = "CourseID";
                chkCourses.DataBind();
            }
        }

        public List<int> SelectedCourseIDs
        {
            get
            {
                return chkCourses.Items.Cast<System.Web.UI.WebControls.ListItem>()
                    .Where(i => i.Selected)
                    .Select(i => int.Parse(i.Value))
                    .ToList();
            }
            set
            {
                foreach (var item in chkCourses.Items.Cast<System.Web.UI.WebControls.ListItem>())
                {
                    item.Selected = value.Contains(int.Parse(item.Value));
                }
            }
        }
    }
}
