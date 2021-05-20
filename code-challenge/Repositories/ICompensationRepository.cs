using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation AddCompensation(Compensation compensation);
        Compensation GetById(String id);
        Task SaveAsync();
    }
}
