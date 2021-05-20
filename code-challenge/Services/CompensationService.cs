using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _logger = logger;
            _compensationRepository = compensationRepository;
        }

        public Compensation AddCompensation(Compensation compensation)
        {
            if(compensation != null)
            {
                _compensationRepository.AddCompensation(compensation);
                _compensationRepository.SaveAsync();
            }
            return compensation;
        }

        public Compensation GetById(String id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return _compensationRepository.GetById(id);
            }
            return null;
        }
    }
}
