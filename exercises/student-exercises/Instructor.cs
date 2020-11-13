using System;

namespace student_exercises
{
    public class Instructor
    {
        public Instructor(string firstName, string lastName, string slackHandle) {
            FirstName = firstName;
            LastName = lastName;
            SlackHandle = slackHandle;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SlackHandle { get; set; }
        public Cohort Cohort {get; set; }

        public void AssignExercise(Exercise exercise, Student student) {
            student.Exercises.Add(exercise);
        }
    }
}
