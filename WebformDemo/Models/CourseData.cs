using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace WebformDemo.Models
{
    public class CourseData
    {
        private string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;

        public List<Course> GetAllCourses()
        {
            List<Course> courses = new List<Course>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Courses";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    courses.Add(new Course
                    {
                        CourseID = (int)reader["CourseId"],
                        CourseName = reader["CourseName"].ToString()
                    });
                }
            }
            return courses;
        }
        public void UpdateCourse(Course course)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "UPDATE Courses SET CourseName = @Name WHERE CourseId = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", course.CourseName);
                cmd.Parameters.AddWithValue("@Id", course.CourseID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void AddCourse(Course course)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO Courses (CourseName) VALUES (@Name)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", course.CourseName);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCourse(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM Courses WHERE CourseId=@Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
