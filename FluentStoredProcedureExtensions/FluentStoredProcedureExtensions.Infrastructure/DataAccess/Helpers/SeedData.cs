using System;
using FluentStoredProcedureExtensions.Core.Entities;
using FluentStoredProcedureExtensions.Infrastructure.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace FluentStoredProcedureExtensions.Infrastructure.DataAccess.Helpers
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Employees.Any())
                {
                    return;
                }

                context.Employees.AddRange(
                    new Employee
                    {
                        Name = "Chewbacca",
                        Email = "Chewbacca@MillenniumFalcon.com"
                    }, new Employee
                    {
                        Name = "Han Solo",
                        Email = "HanSolo@MillenniumFalcon.com"
                    }, new Employee
                    {
                        Name = "Darth Vader",
                        Email = "DarthVader@DarkSide.com"
                    }, new Employee
                    {
                        Name = "Darth Sidious",
                        Email = "DarthSidious@DarkSide.com"
                    }, new Employee
                    {
                        Name = "Obi Wan",
                        Email = "ObiWan@GoodOnes.com"
                    }, new Employee
                    {
                        Name = "Mace Windu",
                        Email = "MaceWindu@GoodOnes.com"
                    });

                context.SaveChanges();
            }
        }
    }
}
