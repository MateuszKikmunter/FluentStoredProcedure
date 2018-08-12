using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Infrastructure.Services;

namespace FluentStoredProcedureExtensions.Infrastructure.Data
{
    public class SqlParameterFactory : ISqlParameterFactory
    {
        private readonly ICollectionToDataTableConverter _collectionToDataTableConverter;

        public SqlParameterFactory(ICollectionToDataTableConverter collectionToDataTableConverter)
        {
            _collectionToDataTableConverter = collectionToDataTableConverter;
        }

        /// <summary>
        /// Creates SqlParameter. 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public SqlParameter CreateParameter(string paramName, object param)
        {
            Guard.ThrowIfStringIsNullOrWhiteSpace(paramName);
            Guard.ThrowIfNull(param);

            return new SqlParameter($"@{ paramName }", param);
        }

        /// <summary>
        /// Creates SqlParameter.
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public SqlParameter CreateParameter(string paramName)
        {
            Guard.ThrowIfStringIsNullOrWhiteSpace(paramName);
            return new SqlParameter { ParameterName = $"@{ paramName }" };
        }

        /// <summary>
        /// Creates user defined table type parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlParameter BuildUserDefinedTableTypeParameter<T>(string paramName, IList<T> value)
        {
            Guard.ThrowIfStringIsNullOrWhiteSpace(paramName);
            Guard.ThrowIfCollectionNullOrEmpty(value);

            return new SqlParameter
            {
                ParameterName = $"@{paramName}",
                SqlDbType = SqlDbType.Structured,
                Value = _collectionToDataTableConverter.ConvertToDataTable(value)
            };
        }
    }
}
