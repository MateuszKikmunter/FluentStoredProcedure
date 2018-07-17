using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Infrastructure.Services;

namespace FluentStoredProcedureExtensions.Infrastructure.Data
{
    public class StoredProcedureFactory : IStoredProcedureFactory
    {
        private readonly ISqlParameterFactory _sqlParameterFactory;

        public StoredProcedureFactory(ISqlParameterFactory sqlParameterFactory)
        {
            _sqlParameterFactory = sqlParameterFactory;
        }

        public IStoredProcedure CreateStoredProcedure(string storedProcedureName)
        {
            Guard.ThrowIfStringIsNullOrWhiteSpace(storedProcedureName);

            var storedProcedure = new StoredProcedure(_sqlParameterFactory);
            storedProcedure.SetStoredProcedureText(storedProcedureName);

            return storedProcedure;
        }
    }
}
