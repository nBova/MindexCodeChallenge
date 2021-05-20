using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;
using System;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpPost]
        /**
         * POST request to add a compensation to the Compensation DB.
         * 
         * Takes a Compensation in the form of JSON, converts it to the object, and adds it to the DB.
         * Returns the status of the request
         */
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for Employee with id: '{compensation.employee.EmployeeId}'");

            _compensationService.AddCompensation(compensation);

            return CreatedAtRoute("getCompensationByEmployeeId", new { id = compensation.employee.EmployeeId }, compensation);
        }

        [HttpGet("{id}", Name = "getCompensationByEmployeeId")]
        /**
         * GET request to get a Compensation given an Employee Id
         * 
         * Uses the compensation service to get the correct Compensation with the given Employee Id as a parameter in the request
         * Returns the compensation along with the status of the request
         */
        public IActionResult GetCompensationByEmployeeId(String id)
        {
            _logger.LogDebug($"Received compensation get request for Employee with id: '{id}'");

            var compensation = _compensationService.GetById(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}