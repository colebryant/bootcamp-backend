using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace student_exercises_revamp.Models
{
    public class Exercise
    {
        public Exercise()
        {
            Students = new List<Student>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Language { get; set; }
        public List<Student> Students { get; set; }
    }
}
