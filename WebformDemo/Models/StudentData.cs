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
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Insert Student
                    string insertStudent = "INSERT INTO Students (Name, RollNo, Class, Section, Email) " +
                                           "OUTPUT INSERTED.StudentID " +
                                           "VALUES (@Name,@RollNo,@Class,@Section,@Email)";

                    SqlCommand cmd = new SqlCommand(insertStudent, con, transaction);
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@RollNo", student.RollNo);
                    cmd.Parameters.AddWithValue("@Class", student.Class);
                    cmd.Parameters.AddWithValue("@Section", student.Section);
                    cmd.Parameters.AddWithValue("@Email", student.Email);

                    int studentId = (int)cmd.ExecuteScalar();

                    // Insert into StudentCourses (multiple courses)
                    foreach (var courseId in student.CourseIds)
                    {
                        string insertCourse = "INSERT INTO StudentCourses (StudentID, CourseID) VALUES (@StudentID, @CourseID)";
                        SqlCommand courseCmd = new SqlCommand(insertCourse, con, transaction);
                        courseCmd.Parameters.AddWithValue("@StudentID", studentId);
                        courseCmd.Parameters.AddWithValue("@CourseID", courseId);
                        courseCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


    }
    public class SchoolData
    {
        private string cs = ConfigurationManager.ConnectionStrings["Student"].ConnectionString;

        // ---------------- Courses ----------------
        public List<Course> GetCourses()
        {
            List<Course> list = new List<Course>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Courses";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Course
                    {
                        CourseID = (int)reader["CourseID"],
                        CourseName = reader["CourseName"].ToString()
                    });
                }
            }
            return list;
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

        public void DeleteCourse(int courseId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM Courses WHERE CourseID=@Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", courseId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ---------------- Teachers ----------------
        public void AddTeacher(Teacher teacher)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Teachers (Name) OUTPUT INSERTED.TeacherID VALUES (@Name)", con);
                cmd.Parameters.AddWithValue("@Name", teacher.TeacherName);
                int teacherId = (int)cmd.ExecuteScalar();

                // Insert into TeacherCourses mapping
                foreach (int courseId in teacher.CourseIds)
                {
                    SqlCommand tcCmd = new SqlCommand("INSERT INTO TeacherCourses (TeacherID, CourseID) VALUES (@TID, @CID)", con);
                    tcCmd.Parameters.AddWithValue("@TID", teacherId);
                    tcCmd.Parameters.AddWithValue("@CID", courseId);
                    tcCmd.ExecuteNonQuery();
                }
            }
        }

        public List<Teacher> GetTeachers()
        {
            List<Teacher> teachers = new List<Teacher>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT T.TeacherID, T.Name, TC.CourseID FROM Teachers T LEFT JOIN TeacherCourses TC ON T.TeacherID=TC.TeacherID";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                var dict = new Dictionary<int, Teacher>();
                while (reader.Read())
                {
                    int tid = (int)reader["TeacherID"];
                    if (!dict.ContainsKey(tid))
                    {
                        dict[tid] = new Teacher
                        {
                            TeacherId = tid,
                            TeacherName = reader["Name"].ToString(),
                            CourseIds = new List<int>()
                        };
                    }
                    if (!(reader["CourseID"] is DBNull))
                    {
                        dict[tid].CourseIds.Add((int)reader["CourseID"]);
                    }
                }
                teachers.AddRange(dict.Values);
            }
            return teachers;
        }
    }
}