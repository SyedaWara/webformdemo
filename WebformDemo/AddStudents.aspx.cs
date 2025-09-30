using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace WebformDemo
{

    public partial class AddStudents : System.Web.UI.Page
    {
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

                // ✅ Check if Email exists
                string checkEmailQuery = "SELECT COUNT(*) FROM Students WHERE Email = @Email";
                SqlCommand checkEmailCmd = new SqlCommand(checkEmailQuery, con);
                checkEmailCmd.Parameters.AddWithValue("@Email", TxtEmail.Text);
                int emailCount = (int)checkEmailCmd.ExecuteScalar();

              


                // ✅ Check if RollNo exists
                string checkRollQuery = "SELECT COUNT(*) FROM Students WHERE RollNo = @RollNo";
                SqlCommand checkRollCmd = new SqlCommand(checkRollQuery, con);
                checkRollCmd.Parameters.AddWithValue("@RollNo", TxtRollNo.Text);
                int rollCount = (int)checkRollCmd.ExecuteScalar();
                if (rollCount > 0 && emailCount > 0)
                {
                    Response.Write("<script>alert('This Roll Number and Email is already registered!');</script>");
                    return;
                }
                else if (emailCount > 0)
                {
                    Response.Write("<script>alert('This Email is already registered!');</script>");
                    return;
                }

                else if (rollCount > 0)
                {
                    Response.Write("<script>alert('This Roll Number is already registered!');</script>");
                    return;
                }
                
                // ✅ If both are unique, Insert record
                string insertQuery = "INSERT INTO Students (Name, RollNo, Class, Section, Email) VALUES (@Name, @RollNo, @Class, @Section, @Email)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, con);

                insertCmd.Parameters.AddWithValue("@Name", TxtName.Text);
                insertCmd.Parameters.AddWithValue("@RollNo", TxtRollNo.Text);
                insertCmd.Parameters.AddWithValue("@Class", TxtClass.Text);
                insertCmd.Parameters.AddWithValue("@Section", TxtSection.Text);
                insertCmd.Parameters.AddWithValue("@Email", TxtEmail.Text);

                insertCmd.ExecuteNonQuery();

                Response.Write("<script>alert('Student added successfully!');</script>");
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewStudent.aspx");
        }

    }
}