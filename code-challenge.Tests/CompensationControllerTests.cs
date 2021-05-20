using challenge.Controllers;
using challenge.Data;
using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        /**
         * This test method just creates a compensation using the post compensation api,
         * and compares the returned compensation to what we originally created.
         */
        public void Create_Compensation_Returns_Created()
        {
            // Make our employee and Compensation
            var employee = new Employee()
            {
                Department = "Test",
                FirstName = "Chris",
                LastName = "Jones",
                Position = "Tester"
            };

            var compensation = new Compensation()
            {
                employee = employee,
                salary = 20000,
                effectiveDate = DateTime.Today
            };

            // Serialize to JSON and send the POST
            var requestContent = new JsonSerialization().ToJson(compensation);
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Asserts
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation);
            Assert.IsNotNull(newCompensation.employee);

            // Compensation specific parameters
            Assert.AreEqual(compensation.salary, newCompensation.salary);
            Assert.AreEqual(compensation.effectiveDate, newCompensation.effectiveDate);

            // Employee parameters
            Assert.AreEqual(employee.FirstName, newCompensation.employee.FirstName);
            Assert.AreEqual(employee.LastName, newCompensation.employee.LastName);
            Assert.AreEqual(employee.Department, newCompensation.employee.Department);
            Assert.AreEqual(employee.Position, newCompensation.employee.Position);
        }

        [TestMethod]
        /**
         * This test method will create a compensation using the post API, and then will
         * retrieve that compensation using the get api and compare the two results.
         * 
         * This test saves from having to seed the Compensation Repository with data
         */
        public void GetCompensationByEmployeeId_Returns_Correct_Compensaiton()
        {
            // Make our employee and Compensation
            var employee = new Employee()
            {
                Department = "Marsellus Wallace",
                FirstName = "Vincent",
                LastName = "Vega",
                Position = "Hitman"
            };

            var compensation = new Compensation()
            {
                employee = employee,
                salary = 100000,
                effectiveDate = DateTime.Today
            };

            // Serialize to JSON and send the POST
            var requestContent = new JsonSerialization().ToJson(compensation);
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var postResponse = postRequestTask.Result;

            var postCompensation = postResponse.DeserializeContent<Compensation>();  // Deserialize so we can send the get with created employee id

            // Send the GET with the returned compensation's employee ID
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{postCompensation.employee.EmployeeId}");
            var getResponse = getRequestTask.Result;

            var getCompensation = getResponse.DeserializeContent<Compensation>(); // Deserialize what we got so we can start comparing

            // Asserts
            // check both status codes
            Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);

            // Check the Compensation parameters
            Assert.AreEqual(postCompensation.salary, getCompensation.salary);
            Assert.AreEqual(compensation.salary, getCompensation.salary);
            Assert.AreEqual(postCompensation.effectiveDate, getCompensation.effectiveDate);
            Assert.AreEqual(compensation.effectiveDate, getCompensation.effectiveDate);
            Assert.AreEqual(postCompensation.CompensationId, getCompensation.CompensationId);

            // Now the Employee parameters
            Assert.AreEqual(postCompensation.employee.EmployeeId, getCompensation.employee.EmployeeId);
            Assert.AreEqual(compensation.employee.FirstName, getCompensation.employee.FirstName);
            Assert.AreEqual(compensation.employee.LastName, getCompensation.employee.LastName);
            Assert.AreEqual(compensation.employee.Department, getCompensation.employee.Department);
            Assert.AreEqual(compensation.employee.Position, getCompensation.employee.Position);
        }
    }
}
