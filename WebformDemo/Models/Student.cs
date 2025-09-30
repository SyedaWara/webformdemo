using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebformDemo.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RollNo { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string Email { get; set; }
    }
}