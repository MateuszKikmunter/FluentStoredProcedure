using FluentStoredProcedure.IntegrationTests.Entities;
using FluentStoredProcedure.IntegrationTests.Entities.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FluentStoredProcedure.IntegrationTests.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }
    }
}
