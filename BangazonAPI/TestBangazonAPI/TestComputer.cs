﻿using BangazonAPI.Models;
using TestBangazonAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestBangazonAPI
{
    public class TestComputer
    {
        [Fact]
        public async Task Test_Get_Computer()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("api/computer");

                string responseBody = await response.Content.ReadAsStringAsync();

                var computerList = JsonConvert.DeserializeObject<List<Computer>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(computerList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Get_Computer_By_Id()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("api/computer/1");

                string responseBody = await response.Content.ReadAsStringAsync();

                var computer = JsonConvert.DeserializeObject<Computer>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(computer.Make);
            }
        }

        [Fact]
        public async Task Test_Post_Computer()
        {
            using (var client = new APIClientProvider().Client)
            {
                Computer computer = new Computer
                {
                    PurchaseDate = new DateTime(2019, 1, 1),
                    DecommissionDate = new DateTime(2019, 1, 2),
                    Make = "XPS",
                    Manufacturer = "Dell"
                };

                var computerJson = JsonConvert.SerializeObject(computer);

                var response = await client.PostAsync(
                    "/api/computer",
                    new StringContent(computerJson, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newComputer = JsonConvert.DeserializeObject<Computer>(responseBody);


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(computer.PurchaseDate, newComputer.PurchaseDate);
                Assert.Equal(computer.DecommissionDate, newComputer.DecommissionDate);
                Assert.Equal(computer.Make, newComputer.Make);
                Assert.Equal(computer.Manufacturer, newComputer.Manufacturer);
            }
        }

        [Fact]
        public async Task Test_Put_Computer()
        {
            string make = "XPS 13";
            using (var client = new APIClientProvider().Client)
            {
                Computer modifiedComputer = new Computer
                {
                    PurchaseDate = new DateTime(2019, 1, 1),
                    DecommissionDate = new DateTime(2019, 1, 2),
                    Make = make,
                    Manufacturer = "Dell"
                };
                var computerJson = JsonConvert.SerializeObject(modifiedComputer);

                var response = await client.PutAsync(
                    "/api/computer/4",
                    new StringContent(computerJson, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                var getComputer = await client.GetAsync("/api/computer/4");
                getComputer.EnsureSuccessStatusCode();

                string getComputerBody = await getComputer.Content.ReadAsStringAsync();
                Computer newComputer = JsonConvert.DeserializeObject<Computer>(getComputerBody);

                Assert.Equal(HttpStatusCode.OK, getComputer.StatusCode);
                Assert.Equal(make, newComputer.Make);
            }
        }

        [Fact]
        public async Task Test_Delete_Computer()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.DeleteAsync("/api/computer/4");

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }
    }
}
