using student_exercises_revamp.Data;
using student_exercises_revamp.Models;
using System;
using System.Collections.Generic;

namespace student_exercises_revamp
{
    class Program
    {
        static void Main(string[] args)
        {
            Repository repository = new Repository();

            //1) Query the database for all the Exercises.
            List<Exercise> exercises = repository.GetAllExercises();
            Console.WriteLine("All Exercises:");
            foreach(Exercise e in exercises)
            {
                Console.WriteLine(e.Name);
            };
            Pause();

            //2) Find all the exercises in the database where the language is JavaScript.
            List<Exercise> javascriptExercises = repository.GetAllExercisesByLanguage("JavaScript");
            Console.WriteLine("JavaScript Exercises:");
            foreach(Exercise e in javascriptExercises)
            {
                Console.WriteLine(e.Name);
            };
            Pause();

            //3) Insert a new exercise into the database.
            Exercise additionalExercise = new Exercise()
            {
                Name = "Departments & Employees",
                Language = "SQL"
            };
            repository.AddExercise(additionalExercise);
            Console.WriteLine("Exercises after adding new one:");
            foreach(Exercise e in repository.GetAllExercises())
            {
                Console.WriteLine(e.Name);
            };
            Pause();

            //4) Find all instructors in the database. Include each instructor's cohort.
            List<Instructor> instructors = repository.GetAllInstructors();
            Console.WriteLine("List of instructors:");
            foreach (Instructor i in instructors)
            {
                Console.WriteLine($"{i.FirstName} {i.LastName} is an instructor in {i.Cohort.Name}");
            }
            Pause();

            //5) Insert a new instructor into the database. Assign the instructor to an existing cohort.
            Instructor additionalInstructor = new Instructor()
            {
                FirstName = "Bill",
                LastName = "Billiamson",
                SlackHandle = "billyboy",
                CohortId = 2
            };
            repository.AddInstructor(additionalInstructor);
            Console.WriteLine("Instructors after adding new one:");
            foreach (Instructor i in repository.GetAllInstructors())
            {
                Console.WriteLine($"{i.FirstName} {i.LastName} is an instructor in {i.Cohort.Name}");
            };
            Pause();

            //6) Assign an existing exercise to an existing student.
            Console.WriteLine("Cole's exercises before assigning new one:");
            foreach(Exercise e in repository.GetStudentExercises(1))
            {
                Console.WriteLine(e.Name);
            }
            repository.AssignExercise(1, 6);
            Console.WriteLine();
            Console.WriteLine("Cole's exercises after assigning new one:");
            foreach (Exercise e in repository.GetStudentExercises(1))
            {
                Console.WriteLine(e.Name);
            }
            Pause();

            //Challenge Question 1) Find all the students in the database. Include each student's cohort AND each student's list of exercises.
            Console.WriteLine("Students in the database:");
            List<Student> students = repository.GetAllStudents();
            foreach(Student s in students)
            {
                Console.WriteLine($"Student: {s.FirstName} {s.LastName}, Cohort: {s.Cohort.Name}");
                Console.WriteLine("Exercises: ");
                foreach(Exercise e in s.Exercises)
                {
                    Console.WriteLine($"{e.Name}");
                }
            }
            Pause();

            //Challenge Question 2) Write a method in the Repository class that accepts an Exercise and a Cohort and assigns that exercise to each student
            //in the cohort IF and ONLY IF the student has not already been assigned the exercise.
            repository.AssignExerciseToCohort("Cohort 30", 5);
            Console.WriteLine("Students in the database after assigning new assignment to Cohort 30:");
            List<Student> newStudents = repository.GetAllStudents();
            foreach (Student s in newStudents)
            {
                Console.WriteLine($"Student: {s.FirstName} {s.LastName}, Cohort: {s.Cohort.Name}");
                Console.WriteLine("Exercises: ");
                foreach (Exercise e in s.Exercises)
                {
                    Console.WriteLine($"{e.Name}");
                }
            }
            Pause();

        }

        public static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
