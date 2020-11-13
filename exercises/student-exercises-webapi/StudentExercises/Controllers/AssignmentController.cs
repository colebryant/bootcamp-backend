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
    public class AssignmentController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AssignmentController(IConfiguration config)
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

        //Assigning an exercise
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StudentExercise studentExercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO StudentExercise (StudentId, ExerciseId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@studentId, @language)";
                    cmd.Parameters.Add(new SqlParameter("@studentId", studentExercise.StudentId));
                    cmd.Parameters.Add(new SqlParameter("@exerciseId", studentExercise.ExerciseId));

                    int newId = (int)cmd.ExecuteScalar();
                    studentExercise.Id = newId;
                    return CreatedAtRoute("GetExerciseById", new { id = newId }, studentExercise);
                }
            }
        }

        //Deleting an Exercise Assignment
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM StudentExercise WHERE Id = @id";
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