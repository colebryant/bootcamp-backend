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
    public class CohortsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CohortsController(IConfiguration config)
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

        //Getting all Cohorts
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Name, s.Id AS StudentId, s.FirstName AS StudentFirst, s.LastName AS StudentLast, s.SlackHandle AS StudentSlack,
                                        i.Id AS InstructorId, i.FirstName AS InstructorFirst, i.LastName AS InstructorLast, i.SlackHandle AS InstructorSlack
                                        FROM Cohort c
                                        LEFT JOIN Student s ON c.Id = s.CohortId
                                        LEFT JOIN Instructor i ON c.Id = i.CohortId";
                    SqlDataReader reader = cmd.ExecuteReader();
                    Dictionary<int, Cohort> cohorts = new Dictionary<int, Cohort>();

                    while (reader.Read())
                    {   
                        if (!cohorts.ContainsKey(reader.GetInt32(reader.GetOrdinal("Id")))) {
                            Cohort cohort = new Cohort()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            cohorts.Add(reader.GetInt32(reader.GetOrdinal("Id")), cohort);
                        };
                        Cohort currentCohort = cohorts[reader.GetInt32(reader.GetOrdinal("Id"))];
                        if (!reader.IsDBNull(reader.GetOrdinal("StudentId"))) {
                            if (!currentCohort.Students.Any(s => s.Id == reader.GetInt32(reader.GetOrdinal("StudentId"))))
                            {
                                Student student = new Student()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("StudentFirst")),
                                    LastName = reader.GetString(reader.GetOrdinal("StudentLast")),
                                    SlackHandle = reader.GetString(reader.GetOrdinal("StudentSlack")),
                                    CohortId = reader.GetInt32(reader.GetOrdinal("Id"))
                                };
                                currentCohort.Students.Add(student);
                            }
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("InstructorId")))
                        {
                            if (!currentCohort.Instructors.Any(i => i.Id == reader.GetInt32(reader.GetOrdinal("InstructorId"))))
                            {
                                Instructor instructor = new Instructor()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("InstructorId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("InstructorFirst")),
                                    LastName = reader.GetString(reader.GetOrdinal("InstructorLast")),
                                    SlackHandle = reader.GetString(reader.GetOrdinal("InstructorSlack")),
                                    CohortId = reader.GetInt32(reader.GetOrdinal("Id"))
                                };
                                currentCohort.Instructors.Add(instructor);
                            }
                        }
                    }
                    reader.Close();
                    return Ok(cohorts);
                }
            }
        }

        //Getting a Cohort by Id
        [HttpGet("{id}", Name="GetCohortById")]
        public async Task<IActionResult> GetCohortById([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Name, s.Id AS StudentId, s.FirstName AS StudentFirst, s.LastName AS StudentLast, s.SlackHandle AS StudentSlack,
                                        i.Id AS InstructorId, i.FirstName AS InstructorFirst, i.LastName AS InstructorLast, i.SlackHandle AS InstructorSlack
                                        FROM Cohort c
                                        LEFT JOIN Student s ON c.Id = s.CohortId
                                        LEFT JOIN Instructor i ON c.Id = i.CohortId
                                        WHERE c.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Cohort cohort = null;

                    while (reader.Read())
                    {
                        if(cohort is null)
                        {
                            cohort = new Cohort()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("StudentId")))
                        {
                            if (!cohort.Students.Any(s => s.Id == reader.GetInt32(reader.GetOrdinal("StudentId"))))
                            {
                                Student student = new Student()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("StudentFirst")),
                                    LastName = reader.GetString(reader.GetOrdinal("StudentLast")),
                                    SlackHandle = reader.GetString(reader.GetOrdinal("StudentSlack")),
                                    CohortId = reader.GetInt32(reader.GetOrdinal("Id"))
                                };
                                cohort.Students.Add(student);
                            }
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("InstructorId")))
                        {
                            if (!cohort.Instructors.Any(i => i.Id == reader.GetInt32(reader.GetOrdinal("InstructorId"))))
                            {
                                Instructor instructor = new Instructor()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("InstructorId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("InstructorFirst")),
                                    LastName = reader.GetString(reader.GetOrdinal("InstructorLast")),
                                    SlackHandle = reader.GetString(reader.GetOrdinal("InstructorSlack")),
                                    CohortId = reader.GetInt32(reader.GetOrdinal("Id"))
                                };
                                cohort.Instructors.Add(instructor);
                            }
                        }
                    }
                    reader.Close();
                    return Ok(cohort);
                }
            }
        }

        //Getting a Cohort with search
        [HttpGet("q={search}", Name = "SearchCohort")]
        public async Task<ActionResult> SearchCohort([FromRoute] string search)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Name, s.Id AS StudentId, s.FirstName AS StudentFirst, s.LastName AS StudentLast, s.SlackHandle AS StudentSlack,
                                        i.Id AS InstructorId, i.FirstName AS InstructorFirst, i.LastName AS InstructorLast, i.SlackHandle AS InstructorSlack
                                        FROM Cohort c
                                        LEFT JOIN Student s ON c.Id = s.CohortId
                                        LEFT JOIN Instructor i ON c.Id = i.CohortId
                                        WHERE c.Name LIKE @search";
                    cmd.Parameters.Add(new SqlParameter("@search", search));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Dictionary<int, Cohort> cohorts = new Dictionary<int, Cohort>();

                    while (reader.Read())
                    {
                        if (!cohorts.ContainsKey(reader.GetInt32(reader.GetOrdinal("Id"))))
                        {
                            Cohort cohort = new Cohort()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            cohorts.Add(reader.GetInt32(reader.GetOrdinal("Id")), cohort);
                        };
                        Cohort currentCohort = cohorts[reader.GetInt32(reader.GetOrdinal("Id"))];
                        if (!reader.IsDBNull(reader.GetOrdinal("StudentId")))
                        {
                            if (!currentCohort.Students.Any(s => s.Id == reader.GetInt32(reader.GetOrdinal("StudentId"))))
                            {
                                Student student = new Student()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("StudentFirst")),
                                    LastName = reader.GetString(reader.GetOrdinal("StudentLast")),
                                    SlackHandle = reader.GetString(reader.GetOrdinal("StudentSlack")),
                                    CohortId = reader.GetInt32(reader.GetOrdinal("Id"))
                                };
                                currentCohort.Students.Add(student);
                            }
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("InstructorId")))
                        {
                            if (!currentCohort.Instructors.Any(i => i.Id == reader.GetInt32(reader.GetOrdinal("InstructorId"))))
                            {
                                Instructor instructor = new Instructor()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("InstructorId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("InstructorFirst")),
                                    LastName = reader.GetString(reader.GetOrdinal("InstructorLast")),
                                    SlackHandle = reader.GetString(reader.GetOrdinal("InstructorSlack")),
                                    CohortId = reader.GetInt32(reader.GetOrdinal("Id"))
                                };
                                currentCohort.Instructors.Add(instructor);
                            }
                        }
                    }
                    reader.Close();
                    return Ok(cohorts);
                }
            }
        }

        //Creating a Cohort
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cohort cohort)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Cohort (Name)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)";
                    cmd.Parameters.Add(new SqlParameter("@name", cohort.Name));

                    int newId = (int)cmd.ExecuteScalar();
                    cohort.Id = newId;
                    return CreatedAtRoute("GetCohortById", new { id = newId }, cohort);
                }
            }
        }

        //Editing a Cohort
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Cohort cohort)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Cohort
                                        SET [Name] = @name
                                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@name", cohort.Name));
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
        //Deleting a cohort
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Cohort WHERE Id = @id";
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
    }
}