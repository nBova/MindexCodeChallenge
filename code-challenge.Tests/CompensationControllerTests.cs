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

            var requestContent = new JsonSerialization().ToJson(compensation);
            Console.WriteLine(requestContent);

            // Execute
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
    }
}
