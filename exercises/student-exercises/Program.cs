using System;
using System.Collections.Generic;
using System.Linq;

namespace student_exercises
{
    class Program
    {
        static void Main(string[] args)
        {
            Exercise FirstExercise = new Exercise("Kennel", "ReactJS");
            Exercise SecondExercise = new Exercise("Bangazon", "C#");
            Exercise ThirdExercise = new Exercise("Welcome to Nashville", "JavaScript");
            Exercise FourthExercise = new Exercise("Superhero", "CSS");

            Student FirstStudent = new Student("Asia", "Carter", "CaptainAsia123");
            Student SecondStudent = new Student("Jordan", "Rosas", "GifMasterFlex");
            Student ThirdStudent = new Student("Joey", "Baumann", "CoderMan5");
            Student FourthStudent = new Student("Maggie", "Leavell", "EveningCoder8");

            Instructor FirstInstructor = new Instructor("Joe", "Sheperd", "TheJoe");
            Instructor SecondInstructor = new Instructor("Andy", "Collins", "GlassesJacketShirtMan");
            Instructor ThirdInstructor = new Instructor("Leah", "Hoefling", "MemeCommander");

            Cohort FirstCohort = new Cohort("Cohort 28");
            FirstInstructor.Cohort = FirstCohort;
            FirstCohort.Instructors.Add(FirstInstructor);

            Cohort SecondCohort = new Cohort("Cohort 29");
            SecondCohort.Students.Add(FirstStudent);
            SecondCohort.Students.Add(SecondStudent);
            SecondCohort.Students.Add(ThirdStudent);
            SecondInstructor.Cohort = SecondCohort;
            ThirdInstructor.Cohort = SecondCohort;
            SecondCohort.Instructors.Add(SecondInstructor);
            SecondCohort.Instructors.Add(ThirdInstructor);

            Cohort ThirdCohort = new Cohort("Cohort E8");
            ThirdCohort.Students.Add(FourthStudent);
            ThirdCohort.Instructors.Add(ThirdInstructor);

            FirstStudent.Cohort = SecondCohort;
            SecondStudent.Cohort = SecondCohort;
            ThirdStudent.Cohort = SecondCohort;
            FourthStudent.Cohort = ThirdCohort;

            SecondInstructor.AssignExercise(FirstExercise, FirstStudent);
            SecondInstructor.AssignExercise(SecondExercise, FirstStudent);
            SecondInstructor.AssignExercise(FirstExercise, SecondStudent);
            SecondInstructor.AssignExercise(ThirdExercise, SecondStudent);
            SecondInstructor.AssignExercise(FirstExercise, ThirdStudent);
            SecondInstructor.AssignExercise(SecondExercise, ThirdStudent);
            SecondInstructor.AssignExercise(ThirdExercise, ThirdStudent);

            ThirdInstructor.AssignExercise(FirstExercise, FourthStudent);
            ThirdInstructor.AssignExercise(SecondExercise, FourthStudent);

            List<Student> students = new List<Student>() {
                FirstStudent,
                SecondStudent,
                ThirdStudent,
                FourthStudent
            };
            List<Exercise> exercises = new List<Exercise>() {
                FirstExercise,
                SecondExercise,
                ThirdExercise,
                FourthExercise
            };
            foreach(Student student in students) {
                List<string> studentExercises = new List<string>();
                foreach(Exercise studentExercise in student.Exercises) {
                    studentExercises.Add(studentExercise.Name);
                }
                Console.WriteLine($"{student.FirstName} {student.LastName} is working on: {String.Join(", ", studentExercises)}");
            }

            List<Instructor> instructors = new List<Instructor>() {
                FirstInstructor,
                SecondInstructor,
                ThirdInstructor
            };

            List<Cohort> cohorts = new List<Cohort>() {
                FirstCohort,
                SecondCohort,
                ThirdCohort,
            };
            Console.WriteLine("-----");
            List<Exercise> javascriptExercises = exercises.Where(e => e.Language == "JavaScript").ToList();
            foreach(Exercise e in javascriptExercises) {
                Console.WriteLine($"{e.Name} is a JavaScript exercise");
            }
            Console.WriteLine("-----");
            List<Student> students29 = students.Where(s => s.Cohort.Name == "Cohort 29").ToList();
            foreach(Student s in students29) {
                Console.WriteLine($"{s.FirstName} {s.LastName} is in Cohort 29");
            }
            Console.WriteLine("-----");
            List<Instructor> instructors29 = instructors.Where(i => i.Cohort.Name == "Cohort 29").ToList();
            foreach(Instructor i in instructors29) {
                Console.WriteLine($"{i.FirstName} {i.LastName} is an instructor in Cohort 29");
            }
            Console.WriteLine("-----");
            List<Student> sortedStudents = students.OrderBy(s => s.LastName).ToList();
            foreach(Student s in sortedStudents) {
                Console.WriteLine($"{s.FirstName} {s.LastName}");
            }
            Student FifthStudent = new Student("Cole", "Bryant", "colebryant");
            FifthStudent.Cohort = SecondCohort;
            students.Add(FifthStudent);
            List<Student> unassignedStudents = students.Where(s => s.Exercises.Count() == 0).ToList();
            Console.WriteLine("-----");
            foreach(Student s in unassignedStudents) {
                Console.WriteLine($"{s.FirstName} {s.LastName} was not assigned any assignments");
            }
            Console.WriteLine("-----");
            Student mostAssigned = students.OrderByDescending(s => s.Exercises.Count()).First();
            Console.WriteLine($"{mostAssigned.FirstName} {mostAssigned.LastName} was assigned the most exercises");
            Console.WriteLine("-----");
            List<string> cohortNumbers = cohorts.Select(c => $"There are currently {c.Students.Count()} student(s) in {c.Name}").ToList();
            foreach(string x in cohortNumbers) {
                Console.WriteLine(x);
            }
        }
    }
}
