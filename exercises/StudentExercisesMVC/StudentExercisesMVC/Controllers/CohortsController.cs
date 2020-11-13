using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;
using StudentExercisesMVC.Models.ViewModels;

namespace StudentExercisesMVC.Controllers
{
    public class CohortsController : Controller
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

        // GET: Cohorts
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Name
                                        FROM Cohort c;";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Cohort> cohorts = new List<Cohort>();

                    while (reader.Read())
                    {
                            Cohort cohort = new Cohort()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            cohorts.Add(cohort);
                    };
                    reader.Close();
                    return View(cohorts);
                }
            }
        }

        // GET: Cohorts/Details/5
        public ActionResult Details(int id)
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
                        if (cohort == null)
                        {
                            cohort = new Cohort()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
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
                    };
                    reader.Close();
                    return View(cohort);
                }
            }
        }

        // GET: Cohorts/Create
        public ActionResult Create()
        {
            CohortCreateViewModel viewModel = new CohortCreateViewModel();

            return View(viewModel);
        }

        // POST: Cohorts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CohortCreateViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Cohort (Name)
                                            VALUES (@name)";
                        cmd.Parameters.Add(new SqlParameter("@name", viewModel.CohortName));

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(viewModel);
            }
        }

        // GET: Cohorts/Edit/5
        public ActionResult Edit(int id)
        {
            Cohort cohort = GetCohortById(id);

            CohortEditViewModel viewModel = new CohortEditViewModel
            {
                CohortName = cohort.Name
            };

            return View(viewModel);
        }

        // POST: Cohorts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CohortEditViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Cohort
                                            SET Name = @name
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@name", viewModel.CohortName));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(viewModel);
            }
        }

        // GET: Cohorts/Delete/5
        public ActionResult Delete(int id)
        {
            Cohort cohort = GetCohortById(id);

            CohortDeleteViewModel viewModel = new CohortDeleteViewModel
            {
                CohortName = cohort.Name
            };

            return View(viewModel);
        }

        // POST: Cohorts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CohortDeleteViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Cohort WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                return View(viewModel);
            }
        }

        private Cohort GetCohortById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name
                                        FROM Cohort
                                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Cohort cohort = null;

                    while (reader.Read())
                    {
                        cohort = new Cohort()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    };
                    reader.Close();
                    return cohort;
                }
            }
        }
    }
}