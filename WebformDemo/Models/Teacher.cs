using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebformDemo.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public List<int> CourseIds { get; set; }   // already present for Add
        public string Courses { get; set; }        // NEW for display
    }

}