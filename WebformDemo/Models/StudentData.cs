using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebformDemo.Models
{
    public class StudentData
    {
        private string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;
        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            using (SqlConnection con  = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Students";
                SqlCommand cmd=new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    students.Add(new Student
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        RollNo = reader["RollNo"].ToString(),
                        Class = reader["Class"].ToString(),
                        Section = reader["Section"].ToString(),
                        Email = reader["Email"].ToString()
                    });

                }    

            }
            return students;
        }

        public void AddStudent(Student student)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO Students (Name, RollNo, Class, Section, Email) VALUES (@Name,@RollNo,@Class,@Section,@Email)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@RollNo", student.RollNo);
                cmd.Parameters.AddWithValue("@Class", student.Class);
                cmd.Parameters.AddWithValue("@Section", student.Section);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}