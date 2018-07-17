using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Infrastructure.Services;

namespace FluentStoredProcedureExtensions.Infrastructure.Data
{
    public class StoredProcedure : IStoredProcedure
    {
        private readonly ISqlParameterFactory _sqlParameterFactory;

        public string StoredProcedureText { get; private set; }
        public IList<SqlParameter> SqlParametersCollection { get; }

        protected StoredProcedure()
        {
        }

        public StoredProcedure(ISqlParameterFactory sqlParameterFactory)
        {
            SqlParametersCollection = SqlParametersCollection ?? new List<SqlParameter>();
            _sqlParameterFactory = sqlParameterFactory;
        }

        public void SetStoredProcedureText(string commandText)
        {
            Guard.ThrowIfStringIsNullOrWhiteSpace(commandText);
            StoredProcedureText = commandText;
        }

        public IStoredProcedure WithSqlParam(string parameterName, Action<SqlParameter> configureParameter = null)
        {
            Guard.ThrowIfStringIsNullOrWhiteSpace(parameterName);

            var parameter = _sqlParameterFactory.CreateParameter(parameterName);
            AppendParameterToCommandText(parameterName);
            ConfigureParamAndAddToCollection(parameter, configureParameter);

            return CreateNewStoredProcedure();
        }

        public IStoredProcedure WithSqlParam(string parameterName, object paramValue, Action<SqlParameter> configureParameter = null)
        {
            Guard.ThrowIfStringIsNullOrWhiteSpace(parameterName);
            Guard.ThrowIfNull(paramValue);

            var parameter = _sqlParameterFactory.CreateParameter(parameterName, paramValue);
            AppendParameterToCommandText(parameterName);
            ConfigureParamAndAddToCollection(parameter, configureParameter);

            return CreateNewStoredProcedure();
        }

        public IStoredProcedure WithUserDefinedDataTableSqlParam<T>(string parameterName, IList<T> paramValue, Action<SqlParameter> configureParameter = null)
        {
            Guard.ThrowIfStringIsNullOrWhiteSpace(parameterName);
            Guard.ThrowIfCollectionNullOrEmpty(paramValue);

            var parameter = _sqlParameterFactory.BuildUserDefinedTableTypeParameter(parameterName, paramValue);
            AppendParameterToCommandText(parameterName);
            ConfigureParamAndAddToCollection(parameter, configureParameter);

            return CreateNewStoredProcedure();
        }

        private IStoredProcedure CreateNewStoredProcedure()
        {
            var storedProcedure = new StoredProcedure(_sqlParameterFactory);
            storedProcedure.SetStoredProcedureText(StoredProcedureText);

            return storedProcedure;
        }

        private void AppendParameterToCommandText(string parameterName)
        {
            StoredProcedureText += $" @{parameterName}, ";
        }

        private void ConfigureParamAndAddToCollection(SqlParameter sqlParameter, Action<SqlParameter> configureParam)
        {
            if (!SqlParametersCollection.Contains(sqlParameter))
            {
                configureParam?.Invoke(sqlParameter);
                SqlParametersCollection.Add(sqlParameter);
            }
        }
    }
}
