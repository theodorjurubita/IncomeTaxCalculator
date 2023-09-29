using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class HrContext : DbContext
    {
        public HrContext()
        {
        }
        public HrContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<TaxBand> TaxBands { get; set; }
    }
}