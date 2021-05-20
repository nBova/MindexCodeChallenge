using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/reporting-structure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet("{id}")]
        /**
         * GET request to find the total number of reports under am employee
         * 
         * Returns a reporting structure and the status of the request
         */
        public IActionResult GetReportingStructure(String id)
        {
            _logger.LogDebug($"Received Reporting Structure get request for '{id}'");

            var reportingStructure = _employeeService.GetReportingStructure(id);

            if (reportingStructure == null)
                return NotFound();

            return Ok(reportingStructure);
        }
    }
}