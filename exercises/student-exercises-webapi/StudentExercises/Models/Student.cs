using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace student_exercises_revamp.Models
{
    public class Student
    {
        public Student()
        {
            Exercises = new List<Exercise>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [StringLength(12, MinimumLength = 3)]
        public string SlackHandle { get; set; }
        public int CohortId { get; set; }
        public Cohort Cohort { get; set; }
        public List<Exercise> Exercises { get; set; }
    }
}
