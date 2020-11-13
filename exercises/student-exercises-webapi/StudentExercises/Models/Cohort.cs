using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace student_exercises_revamp.Models
{
    public class Cohort
    {
        public Cohort()
        {
            Students = new List<Student>();
            Instructors = new List<Instructor>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(11, MinimumLength =5)]
        public string Name { get; set; }
        public List<Student> Students { get; set; }
        public List<Instructor> Instructors { get; set; }
    }
}
