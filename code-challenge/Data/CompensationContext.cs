using challenge.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Data
{
    /**
     * I relaize this could be a bit overkill but in my eyes, Compensation is an entirely different data type than
     * Employee, even if it contains an Employee. If this were a huge project, work would need to be done to make sure
     * that every Compensation contains a valid Employee (an Employee that exists in the Employee Database).
     */
    public class CompensationContext : DbContext
    {
        public CompensationContext(DbContextOptions<CompensationContext> options) : base(options) { }

        public DbSet<Compensation> Compensations { get; set; }
    }
}