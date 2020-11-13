using Newtonsoft.Json;
using student_exercises_revamp.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudentExercises.Tests
{
    public class TestInstructors
    {
        [Fact]
        public async Task Test_Get_Instructor()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("api/instructors");

                string responseBody = await response.Content.ReadAsStringAsync();

                var students = JsonConvert.DeserializeObject<List<Instructor>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(students.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Post_Instructor()
        {
            using (var client = new APIClientProvider().Client)
            {
                Instructor instructor = new Instructor
                {
                    FirstName = "Jimmy",
                    LastName = "Hendrix",
                    SlackHandle = "guitarman",
                    CohortId = 1
                };

                var instructorJson = JsonConvert.SerializeObject(instructor);

                var response = await client.PostAsync(
                    "/api/instructors",
                    new StringContent(instructorJson, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newInstructor = JsonConvert.DeserializeObject<Instructor>(responseBody);


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(instructor.FirstName, newInstructor.FirstName);
                Assert.Equal(instructor.LastName, newInstructor.LastName);
                Assert.Equal(instructor.SlackHandle, newInstructor.SlackHandle);
                Assert.Equal(instructor.CohortId, newInstructor.CohortId);
            }
        }

        [Fact]
        public async Task Test_Put_Instructor()
        {
            string newFirstName = "Andrew";
            using (var client = new APIClientProvider().Client)
            {
                Instructor modifiedInstructor = new Instructor
                {
                    FirstName = newFirstName,
                    LastName = "Collins",
                    SlackHandle = "acollins",
                    CohortId = 1

                };
                var instructorJson = JsonConvert.SerializeObject(modifiedInstructor);

                var response = await client.PutAsync(
                    "/api/instructors/1",
                    new StringContent(instructorJson, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                var getInstructor = await client.GetAsync("/api/instructors/1");
                getInstructor.EnsureSuccessStatusCode();

                string getInstructorBody = await getInstructor.Content.ReadAsStringAsync();
                Student newInstructor = JsonConvert.DeserializeObject<Student>(getInstructorBody);

                Assert.Equal(HttpStatusCode.OK, getInstructor.StatusCode);
                Assert.Equal(newFirstName, newInstructor.FirstName);
            }
        }

        [Fact]
        public async Task Test_Delete_Instructor()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.DeleteAsync("/api/instructors/5");


                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }
    }
}
