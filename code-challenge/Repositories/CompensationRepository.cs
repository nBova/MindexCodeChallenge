using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<CompensationRepository> _logger;

        public CompensationRepository(ILogger<CompensationRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        /**
         * Very simple method to add a Compensation to the Compensation Database.
         * Creates a unique GUID for the compensation so that it may be stored.
         * 
         * Returns the created Compensation with it's new GUID
         */
        public Compensation AddCompensation(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }

        /**
         * Gets and Returns a Compensation given an Employee Id. 
         * This is assuming there are no two Compensations with the same Employee.
         */
        public Compensation GetById(String id)
        {
            // We want to make sure we include the employee in the GET
            return _compensationContext.Compensations.Include(e => e.employee).SingleOrDefault(e => e.employee.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
