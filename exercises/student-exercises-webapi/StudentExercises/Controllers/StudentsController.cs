using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using student_exercises_revamp.Models;

namespace StudentExercises.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public StudentsController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        
        //Get all Students (and include exercises if include=exercise is added)
        [HttpGet]
        public async Task<IActionResult> Get(string include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "exercise")
                    {
                        cmd.CommandText = @"SELECT s.Id, s.FirstName, s.LastName, s.SlackHandle, c.Name as CohortName,
                                    e.Id AS ExerciseId, e.Name AS ExerciseName, e.Language
                                    FROM Student s
                                    LEFT JOIN Cohort c ON s.CohortId = c.Id
                                    LEFT JOIN StudentExercise se ON c.Id = se.StudentId
                                    LEFT JOIN Exercise e ON se.ExerciseId = e.Id";
                        SqlDataReader reader = cmd.ExecuteReader();
                        Dictionary<int, Student> students = new Dictionary<int, Student>();

                        while (reader.Read())
                        {
                            int studentId = reader.GetInt32(reader.GetOrdinal("Id"));
                            if (!students.ContainsKey(studentId))
                            {
                                Student student = new Student()
                                {
                                    Id = studentId,
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle"))
                                };
                                students.Add(studentId, student);
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("ExerciseId")))
                            {
                                    Student currentStudent = students[studentId];
                                    currentStudent.Exercises.Add(new Exercise
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                                        Name = reader.GetString(reader.GetOrdinal("ExerciseName")),
                                        Language = reader.GetString(reader.GetOrdinal("Language"))
                                    }
                                );
                            }
                        }
                        reader.Close();
                        return (Ok(students));
                    }
                    else
                    {
                        cmd.CommandText = @"SELECT s.Id, s.FirstName, s.LastName, s.SlackHandle, c.Name AS CohortName, c.Id AS CohortId FROM Student s
                                        LEFT JOIN Cohort c ON s.CohortId = c.Id";
                        SqlDataReader reader = cmd.ExecuteReader();
                        List<Student> students = new List<Student>();

                        while (reader.Read())
                        {
                            Student student = new Student()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                                CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                Cohort = new Cohort()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                    Name = reader.GetString(reader.GetOrdinal("CohortName"))
                                }
                            };
                            students.Add(student);
                        }
                        reader.Close();
                        return Ok(students);
                    }
                }
            }
        }

        //Getting a Student by Id
        [HttpGet("{id}", Name = "GetStudentWithId")]
        public async Task<ActionResult> GetStudentWithId([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id, s.FirstName, s.LastName, s.SlackHandle, s.CohortId, c.Name as CohortName FROM Student s
                                        LEFT JOIN Cohort c ON s.CohortId = c.Id
                                        WHERE s.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Student student = null;

                    while (reader.Read())
                    {
                        student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = new Cohort()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                Name = reader.GetString(reader.GetOrdinal("CohortName"))
                            }
                        };
                    }
                    reader.Close();

                    return (Ok(student));
                }
            }
        }

        //Getting a Student with search
        [HttpGet("q={search}", Name = "SearchStudent")]
        public async Task<ActionResult> SearchStudent([FromRoute] string search)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id, s.FirstName, s.LastName, s.SlackHandle, s.CohortId, c.Name as CohortName FROM Student s
                                        LEFT JOIN Cohort c ON s.CohortId = c.Id
                                        WHERE s.FirstName LIKE @search OR s.LastName LIKE @search OR s.SlackHandle LIKE @search";
                    cmd.Parameters.Add(new SqlParameter("@search", search));
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> students = new List<Student>();

                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = new Cohort()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                Name = reader.GetString(reader.GetOrdinal("CohortName"))
                            }
                        };
                        students.Add(student);
                    }
                    reader.Close();

                    return (Ok(students));
                }
            }
        }

        //Creating a Student
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student student)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@firstName, @lastName, @slackHandle, @cohortId)";
                    cmd.Parameters.Add(new SqlParameter("@firstName", student.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", student.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slackHandle", student.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", student.CohortId));

                    int newId = (int)cmd.ExecuteScalar();
                    student.Id = newId;
                    return CreatedAtRoute("GetStudentWithId", new { id = newId }, student);
                }
            }
        }

        //Editing a Student
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Student student)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Student
                                        SET FirstName = @firstName,
                                            LastName = @lastName,
                                            SlackHandle = @slackHandle,
                                            CohortId = @cohortId
                                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@firstName", student.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", student.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slackHandle", student.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", student.CohortId));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return new StatusCodeResult(StatusCodes.Status204NoContent);
                    }
                    throw new Exception("No rows affected");
                }
            }
        }

        //Deleting a Student
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Student WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return NoContent();
                    }
                    throw new Exception("No rows affected");
                }
            }
        }
    }
}
