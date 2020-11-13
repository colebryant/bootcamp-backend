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
    public class ExercisesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ExercisesController(IConfiguration config)
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

        //Getting all Exercises
        [HttpGet]
        public async Task<IActionResult> Get(string include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "student")
                    {
                        cmd.CommandText = @"SELECT e.Id, e.Name, e.Language, s.FirstName, s.LastName, s.SlackHandle, 
                                            s.Id AS StudentId, s.CohortId
                                            FROM Exercise e
                                            LEFT JOIN StudentExercise se ON e.Id = se.ExerciseId
                                            LEFT JOIN Student s ON se.StudentId = s.Id";
                        SqlDataReader reader = cmd.ExecuteReader();
                        Dictionary<int, Exercise> exercises = new Dictionary<int, Exercise>();

                        while (reader.Read())
                        {
                            int exerciseId = reader.GetInt32(reader.GetOrdinal("Id"));
                            if (!exercises.ContainsKey(exerciseId))
                            {
                                Exercise exercise = new Exercise()
                                {
                                    Id = exerciseId,
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Language = reader.GetString(reader.GetOrdinal("Language"))
                                };
                                exercises.Add(exerciseId, exercise);
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("StudentId")))
                            {
                                Exercise currentExercise = exercises[exerciseId];
                                currentExercise.Students.Add(new Student
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                                    CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                                });
                            }
                        }
                        reader.Close();
                        return (Ok(exercises));

                    }
                    else
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

                        return (Ok(exercises));
                    }
                }
            }
        }
        
        //Getting an Exercise by Id
        [HttpGet("{id}", Name ="GetExerciseById")]
        public async Task<ActionResult> GetExerciseById([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM Exercise
                                        WHERE Exercise.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Exercise exercise = null;

                    while (reader.Read())
                    {
                        exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };
                    }
                    reader.Close();

                    return (Ok(exercise));
                }
            }
        }

        //Getting an Exercise with search
        [HttpGet("q={search}", Name = "SearchExercise")]
        public async Task<ActionResult> SearchExercise([FromRoute] string search)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT e.Id, e.Name, e.Language FROM Exercise e
                                        WHERE e.Name LIKE @search OR e.Language LIKE @search";
                    cmd.Parameters.Add(new SqlParameter("@search", search));
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

                    return (Ok(exercises));
                }
            }
        }

        //Creating an Exercise
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Exercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Exercise (Name, Language)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name, @language)";
                    cmd.Parameters.Add(new SqlParameter("@name", exercise.Name));
                    cmd.Parameters.Add(new SqlParameter("@language", exercise.Language));

                    int newId = (int)cmd.ExecuteScalar();
                    exercise.Id = newId;
                    return CreatedAtRoute("GetExerciseById", new { id = newId }, exercise);
                }
            }
        }

        //Editing an Exercise
        [HttpPut("{id}")]
            public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Exercise exercise)
            {
                try
                {
                    using (SqlConnection conn = Connection)
                    {
                        conn.Open();
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = @"UPDATE Exercise
                                                SET Name = @name,
                                                    Language = @language
                                                WHERE Id = @id";
                            cmd.Parameters.Add(new SqlParameter("@name", exercise.Name));
                            cmd.Parameters.Add(new SqlParameter("@language", exercise.Language));
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
                catch (Exception)
                {
                    if(!ExerciseExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

        //Deleting an Exercise
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Exercise WHERE Id = @id";
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
            catch (Exception)
            {
                if (!ExerciseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool ExerciseExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM Exercise
                                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}