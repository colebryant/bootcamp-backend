using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StudentExercisesMVC.Models
{
    public class StudentExercise
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ExerciseId { get; set; }
    }
}
    