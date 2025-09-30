using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebformDemo
{
    public partial class StudentDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.QueryString["StudentID"] != null)
            {
                int studentID = Convert.ToInt32(Request.QueryString["StudentID"]);
                LoadStudent(studentID);
            }
        }
        private void LoadStudent(int studentID)
        {
            string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Students WHERE StudentID=@StudentID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentID", studentID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblDetails.Text =
                        "ID: " + reader["StudentID"] + "<br/>" +
                        "Name: " + reader["Name"] + "<br/>" +
                        "Roll No: " + reader["RollNo"] + "<br/>" +
                        "Class: " + reader["Class"] + "<br/>" +
                        "Section: " + reader["Section"] + "<br/>" +
                        "Email: " + reader["Email"];
                }
            }
        }
    }
}