using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentStoredProcedureExtensions.Core.Abstract;
using Microsoft.EntityFrameworkCore;

namespace FluentStoredProcedureExtensions.Infrastructure.Extensions
{
    public static class DbContextExtensionss
    {
        public static async Task<int> ExecuteSqlCommandAsync(this DbContext context, IStoredProcedure storedProcedure)
        {
            return await context
                .Database
                .ExecuteSqlCommandAsync(storedProcedure.StoredProcedureText, storedProcedure.SqlParametersCollection);
        }

        public static async Task<IEnumerable<T>> FromSqlAsync<T>(this DbContext context, IStoredProcedure storedProcedure) where T : class
        {
            return await context
                .SetEntities<T>()
                .FromSql(storedProcedure.StoredProcedureText, storedProcedure.SqlParametersCollection.ToArray())
                .ToListAsync();
        }

        public static int ExecuteSqlCommand(this DbContext context, IStoredProcedure storedProcedure)
        {
            return context
                .Database
                .ExecuteSqlCommand(storedProcedure.StoredProcedureText, storedProcedure.SqlParametersCollection);
        }

        public static IEnumerable<T> FromSql<T>(this DbContext context, IStoredProcedure storedProcedure) where T : class
        {
            return context
                .SetEntities<T>()
                .FromSql(storedProcedure.StoredProcedureText, storedProcedure.SqlParametersCollection.ToArray())
                .ToList();
        }

        private static DbSet<T> SetEntities<T>(this DbContext context) where T : class
        {
            return context.Set<T>();
        }
    }
}
