using student_exercises_revamp.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace student_exercises_revamp.Data
{
    class Repository
    {
        public SqlConnection Connection
        {
            get
            {
                string _connectionString = "Server=DESKTOP-0ILPN6E\\SQLEXPRESS;Database=StudentExercisesDB;Trusted_Connection=True;";
                return new SqlConnection(_connectionString);
            }
        }
        
        public List<Exercise> GetAllExercises()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Exercise";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Exercise> exercises = new List<Exercise>();
                    while (reader.Read())
                    {
                        Exercise exercise = new Exercise()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };
                        exercises.Add(exercise);
                    }
                    reader.Close();
                    return exercises;
                }
            }
        }

        public List<Exercise> GetAllExercisesByLanguage(string language)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM Exercise e
                                         WHERE e.Language = @language";
                    cmd.Parameters.Add(new SqlParameter("@language", language));

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Exercise> exercises = new List<Exercise>();
                    while (reader.Read())
                    {
                        Exercise exercise = new Exercise()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };
                        exercises.Add(exercise);
                    }
                    reader.Close();
                    return exercises;
                }
            }
        }

        public void AddExercise (Exercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Exercise
                                         VALUES (@exerciseName, @exerciseLanguage)";
                    cmd.Parameters.Add(new SqlParameter("@exerciseName", exercise.Name));
                    cmd.Parameters.Add(new SqlParameter("@exerciseLanguage", exercise.Language));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Instructor> GetAllInstructors()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM Instructor i
                                        INNER JOIN Cohort c ON c.id = i.CohortId";

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Instructor> instructors = new List<Instructor>();
                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        Instructor instructor = new Instructor()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = cohort
                        };
                        instructors.Add(instructor);
                    }
                    reader.Close();
                    return instructors;
                }
            }
        }

        public void AddInstructor(Instructor instructor)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Instructor
                                         VALUES (@firstName, @lastName, @slackHandle, @cohortId)";
                    cmd.Parameters.Add(new SqlParameter("@firstName", instructor.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", instructor.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slackHandle", instructor.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", instructor.CohortId));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AssignExercise(int studentId, int exerciseId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO StudentExercise
                                         VALUES (@studentId, @exerciseId)";
                    cmd.Parameters.Add(new SqlParameter("@studentId", studentId));
                    cmd.Parameters.Add(new SqlParameter("@exerciseId", exerciseId));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Exercise> GetStudentExercises(int studentId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM StudentExercise s
                                         INNER JOIN Exercise e ON e.Id = s.ExerciseId
                                         WHERE s.StudentId = @studentId";
                    cmd.Parameters.Add(new SqlParameter("@studentId", studentId));

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Exercise> exercises = new List<Exercise>();
                    while (reader.Read())
                    {
                        Exercise exercise = new Exercise()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };
                        exercises.Add(exercise);
                    }
                    reader.Close();
                    return exercises;
                }
            }
        }

        public List<Student> GetAllStudents()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT *, s.Id AS StudentId FROM Student s
                                         INNER JOIN Cohort c ON c.Id = s.CohortId";

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Student> students = new List<Student>();
                    while (reader.Read())
                    {
                        List<Exercise> exercises = GetStudentExercises(reader.GetInt32(reader.GetOrdinal("StudentId")));
                        Cohort cohort = new Cohort()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        Student student = new Student()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = cohort,
                            Exercises = exercises
                        };
                        students.Add(student);
                    }
                    reader.Close();
                    return students;
                }
            }
        }

        public void AssignExerciseToCohort(string cohortName, int exerciseId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    List<Student> students = GetAllStudents().Where(s => s.Cohort.Name == cohortName).ToList();
                    int i = 0;
                    foreach (Student s in students)
                    {
                        if(!s.Exercises.Any(e => e.Id == exerciseId)) {
                        i++;
                        cmd.CommandText = $@"INSERT INTO StudentExercise
                                             VALUES (@cohortStudentId{i}, @cohortExerciseId{i})";
                        cmd.Parameters.Add(new SqlParameter($"@cohortStudentId{i}", s.Id));
                        cmd.Parameters.Add(new SqlParameter($"@cohortExerciseId{i}", exerciseId));
                        cmd.ExecuteNonQuery();
                        }
                    };
                }
            }
        }


    }
}
