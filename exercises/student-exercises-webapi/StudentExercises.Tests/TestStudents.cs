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
    public class TestStudents
    {
        [Fact]
        public async Task Test_Get_Student()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("api/students");

                string responseBody = await response.Content.ReadAsStringAsync();

                var students = JsonConvert.DeserializeObject<List<Student>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(students.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Post_Student()
        {
            using (var client = new APIClientProvider().Client)
            {
                Student student = new Student
                {
                    FirstName = "Stevie",
                    LastName = "Wonder",
                    SlackHandle = "FunkMaster",
                    CohortId = 1
                };

                var studentJson = JsonConvert.SerializeObject(student);

                var response = await client.PostAsync(
                    "/api/students",
                    new StringContent(studentJson, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newStudent = JsonConvert.DeserializeObject<Student>(responseBody);


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(student.FirstName, newStudent.FirstName);
                Assert.Equal(student.LastName, newStudent.LastName);
                Assert.Equal(student.SlackHandle, newStudent.SlackHandle);
                Assert.Equal(student.CohortId, newStudent.CohortId);
            }
        }

        [Fact]
        public async Task Test_Put_Student()
        {
            string newFirstName = "Bob";
            using (var client = new APIClientProvider().Client)
            {
                Student modifiedStudent = new Student
                {
                    FirstName = newFirstName,
                    LastName = "Bryant",
                    SlackHandle = "colebryant",
                    CohortId = 1

                };
                var studentJson = JsonConvert.SerializeObject(modifiedStudent);

                var response = await client.PutAsync(
                    "/api/students/1",
                    new StringContent(studentJson, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                var getStudent = await client.GetAsync("/api/students/1");
                getStudent.EnsureSuccessStatusCode();

                string getStudentBody = await getStudent.Content.ReadAsStringAsync();
                Student newStudent = JsonConvert.DeserializeObject<Student>(getStudentBody);

                Assert.Equal(HttpStatusCode.OK, getStudent.StatusCode);
                Assert.Equal(newFirstName, newStudent.FirstName);
            }
        }

        [Fact]
        public async Task Test_Delete_Student()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.DeleteAsync("/api/students/11");


                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }
    }
}
