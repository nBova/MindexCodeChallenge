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
    public class ReportingStructureControllerTests
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
         * Executes a GET request for a reporting structure and compares to the expected values
         */
        public void GetReportingStructure_Test()
        {
            // Get together our expected Employee data
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";
            var expectedNumberReports = 4;

            // Send GET request and deserialize JSON result
            var getRequestTask = _httpClient.GetAsync($"api/reporting-structure/{employeeId}");
            var response = getRequestTask.Result;
            var reportingStructure = response.DeserializeContent<ReportingStructure>();

            // Asserts
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(employeeId, reportingStructure.employee.EmployeeId);
            Assert.AreEqual(expectedFirstName, reportingStructure.employee.FirstName);
            Assert.AreEqual(expectedLastName, reportingStructure.employee.LastName);
            Assert.AreEqual(expectedNumberReports, reportingStructure.numberOfReports);
        }

    }
}
