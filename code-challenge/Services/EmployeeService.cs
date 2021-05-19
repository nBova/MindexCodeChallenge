using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        /**
         * Creates and returns a ReportingStructure given an Employee id.
         */
        public ReportingStructure GetReportingStructure(String id)
        {
            Employee employee = GetById(id);
            int count = GetTotalReports(id);
            return new ReportingStructure { employee = employee, numberOfReports = count };
        }

        /**
         * Recursively goes through the base employee and their direct reports, returns the total number
         * of direct reports under the given employee ID.
         */
        private int GetTotalReports(String id)
        {
            Employee employee = GetById(id); // Get the Employee so we can look through their direct reports
            int count = 0;
            foreach(Employee directReport in employee.DirectReports)
            {
                count += 1 + GetTotalReports(directReport.EmployeeId);  // Increment for each Employee found, also check their direct reports
            }
            return count;
        }
    }
}
