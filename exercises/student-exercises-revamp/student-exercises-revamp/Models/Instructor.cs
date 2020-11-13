using System;
using System.Collections.Generic;
using System.Text;

namespace student_exercises_revamp.Models
{
    class Instructor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SlackHandle { get; set; }
        public int CohortId { get; set; }
        public Cohort Cohort { get; set; }
    }
}
