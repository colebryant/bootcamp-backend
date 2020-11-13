using System;
using System.Collections.Generic;

namespace student_exercises
{
    public class Student
    {
        public Student(string firstName, string lastName, string slackHandle) {
            FirstName = firstName;
            LastName = lastName;
            SlackHandle = slackHandle;
            Exercises = new List<Exercise>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SlackHandle { get; set; }
        public Cohort Cohort {get; set; }
        public List<Exercise> Exercises { get; set; }
    }
}
