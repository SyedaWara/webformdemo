using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebformDemo.Models
{
    public class CourseTeacher
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
    }

}