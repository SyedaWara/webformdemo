using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace WebformDemo.Models
{
    public class TeacherData
    {
        private string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;

        public List<Teacher> GetAllTeachers()
        {
            List<Teacher> teachers = new List<Teacher>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Teachers";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    teachers.Add(new Teacher
                    {
                        TeacherId = (int)reader["TeacherId"],
                        TeacherName = reader["TeacherName"].ToString()
                    });
                }
            }
            return teachers;
        }

        public void AddTeacher(Teacher teacher)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO Teachers (TeacherName) VALUES (@Name); SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", teacher.TeacherName);
                con.Open();
                int teacherId = Convert.ToInt32(cmd.ExecuteScalar());

                // Assign courses
                foreach (var courseId in teacher.CourseIds)
                {
                    SqlCommand mapCmd = new SqlCommand("INSERT INTO TeacherCourses (TeacherId, CourseId) VALUES (@TId, @CId)", con);
                    mapCmd.Parameters.AddWithValue("@TId", teacherId);
                    mapCmd.Parameters.AddWithValue("@CId", courseId);
                    mapCmd.ExecuteNonQuery();
                }
            }
        }
        public List<Teacher> GetTeachersByCourse(int courseId)
        {
            List<Teacher> teachers = new List<Teacher>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT t.TeacherID, t.TeacherName
                         FROM TeacherCourses tc
                         INNER JOIN Teachers t ON tc.TeacherID = t.TeacherID
                         WHERE tc.CourseID = @CourseID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CourseID", courseId);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    teachers.Add(new Teacher
                    {
                        TeacherId = (int)reader["TeacherID"],
                        TeacherName = reader["TeacherName"].ToString()
                    });
                }
            }

            return teachers;
        }

    }
}
