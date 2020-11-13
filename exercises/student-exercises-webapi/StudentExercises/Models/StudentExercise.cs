using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace student_exercises_revamp.Models
{
    public class StudentExercise
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ExerciseId { get; set; }
    }
}
    