using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebformDemo
{
    public partial class ViewStudent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Students", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        // Edit Row
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        // Cancel Edit
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }

        // Update Row
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int studentID = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            GridViewRow row = GridView1.Rows[e.RowIndex];
            string name = (row.FindControl("txtName") as TextBox).Text;
            string rollNo = (row.FindControl("txtRollNo") as TextBox).Text;
            string className = (row.FindControl("txtClass") as TextBox).Text;
            string section = (row.FindControl("txtSection") as TextBox).Text;
            string email = (row.FindControl("txtEmail") as TextBox).Text;

            string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "UPDATE Students SET Name=@Name, RollNo=@RollNo, Class=@Class, Section=@Section, Email=@Email WHERE StudentID=@StudentID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@RollNo", rollNo);
                cmd.Parameters.AddWithValue("@Class", className);
                cmd.Parameters.AddWithValue("@Section", section);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@StudentID", studentID);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            GridView1.EditIndex = -1;
            BindGrid();
        }

        // Delete Row
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int studentID = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM Students WHERE StudentID=@StudentID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentID", studentID);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            BindGrid();
        }

        // Handle paging
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        protected void GridView1_SelectedIndexChanged(object sender,EventArgs e)
        {
            int studentID = Convert.ToInt32(GridView1.SelectedDataKey.Value);
            Response.Redirect("StudentDetails.aspx?StudentID=" + studentID);
        }
        protected void Add_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddStudents.aspx");

        }
    }
}
