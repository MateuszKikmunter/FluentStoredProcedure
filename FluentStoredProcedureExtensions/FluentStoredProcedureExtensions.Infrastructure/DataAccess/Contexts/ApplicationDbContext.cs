using FluentStoredProcedureExtensions.Core.Entities;
using FluentStoredProcedureExtensions.Infrastructure.DataAccess.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;

namespace FluentStoredProcedureExtensions.Infrastructure.DataAccess.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }
    }
}
