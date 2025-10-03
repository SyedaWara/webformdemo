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
                string query = @"
                    SELECT t.TeacherId, 
                           t.TeacherName, 
                           STRING_AGG(c.CourseName, ', ') AS Courses
                    FROM Teachers t
                    LEFT JOIN TeacherCourses tc ON t.TeacherId = tc.TeacherId
                    LEFT JOIN Courses c ON tc.CourseId = c.CourseId
                    GROUP BY t.TeacherId, t.TeacherName";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    teachers.Add(new Teacher
                    {
                        TeacherId = Convert.ToInt32(reader["TeacherId"]),
                        TeacherName = reader["TeacherName"].ToString(),
                        Courses = reader["Courses"] == DBNull.Value ? "" : reader["Courses"].ToString()
                    });
                }
            }
            return teachers;
        }

        public void AddTeacher(Teacher teacher)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    string query = "INSERT INTO Teachers (TeacherName) VALUES (@Name); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmd = new SqlCommand(query, con, tran);
                    cmd.Parameters.AddWithValue("@Name", teacher.TeacherName);

                    int teacherId = Convert.ToInt32((decimal)cmd.ExecuteScalar());

                    foreach (var courseId in teacher.CourseIds)
                    {
                        SqlCommand mapCmd = new SqlCommand(
                            "INSERT INTO TeacherCourses (TeacherId, CourseId) VALUES (@TId, @CId)", con, tran);
                        mapCmd.Parameters.AddWithValue("@TId", teacherId);
                        mapCmd.Parameters.AddWithValue("@CId", courseId);
                        mapCmd.ExecuteNonQuery();
                    }

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
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
        public void DeleteTeacher(int teacherId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // First delete teacher-course mappings
                string deleteMappings = "DELETE FROM TeacherCourses WHERE TeacherId = @TeacherId";
                SqlCommand cmd1 = new SqlCommand(deleteMappings, con);
                cmd1.Parameters.AddWithValue("@TeacherId", teacherId);
                cmd1.ExecuteNonQuery();

                // Then delete teacher
                string deleteTeacher = "DELETE FROM Teachers WHERE TeacherID = @TeacherId";
                SqlCommand cmd2 = new SqlCommand(deleteTeacher, con);
                cmd2.Parameters.AddWithValue("@TeacherId", teacherId);
                cmd2.ExecuteNonQuery();
            }
        }
    }
}
