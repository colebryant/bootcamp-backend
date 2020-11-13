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
    public class TestExercises
    {
        [Fact]
        public async Task Test_Get_Exercise()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("api/exercises");

                string responseBody = await response.Content.ReadAsStringAsync();

                var exercises = JsonConvert.DeserializeObject<List<Exercise>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(exercises.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Post_Exercise()
        {
            using (var client = new APIClientProvider().Client)
            {
                Exercise exercise = new Exercise
                {
                    Name = "Back-end Capstone",
                    Language = "C#"
                };

                var exerciseJson = JsonConvert.SerializeObject(exercise);

                var response = await client.PostAsync(
                    "/api/exercises",
                    new StringContent(exerciseJson, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newExercise = JsonConvert.DeserializeObject<Exercise>(responseBody);


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(exercise.Name, newExercise.Name);
                Assert.Equal(exercise.Language, newExercise.Language);
            }
        }

        [Fact]
        public async Task Test_Put_Exercise()
        {
            string name = "Back-end Final Project";
            using (var client = new APIClientProvider().Client)
            {
                Exercise modifiedExercise = new Exercise
                {
                    Name = name,
                    Language = "C#"
                };
                var exerciseJson = JsonConvert.SerializeObject(modifiedExercise);

                var response = await client.PutAsync(
                    "/api/exercises/7",
                    new StringContent(exerciseJson, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                var getExercise = await client.GetAsync("/api/exercises/7");
                getExercise.EnsureSuccessStatusCode();

                string getExerciseBody = await getExercise.Content.ReadAsStringAsync();
                Exercise newExercise = JsonConvert.DeserializeObject<Exercise>(getExerciseBody);

                Assert.Equal(HttpStatusCode.OK, getExercise.StatusCode);
                Assert.Equal(name, newExercise.Name);
            }
        }

        [Fact]
        public async Task Test_Delete_Exercise()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.DeleteAsync("/api/exercises/7");


                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }
    }
}
