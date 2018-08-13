using FluentStoredProcedureExtensions.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FluentStoredProcedureExtensions.Infrastructure.DataAccess.EntitiesConfigurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Email).IsRequired().HasMaxLength(255);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(155);

            builder.ToTable("Employees");
        }
    }
}
