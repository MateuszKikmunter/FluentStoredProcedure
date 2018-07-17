using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FluentStoredProcedureExtensions.Core.Abstract
{
    public interface IStoredProcedure
    {
        string StoredProcedureText { get; }
        IList<SqlParameter> SqlParametersCollection { get; }
        void SetStoredProcedureText(string commandText);
        IStoredProcedure WithSqlParam(string parameterName, Action<SqlParameter> configureParameter = null);
        IStoredProcedure WithSqlParam(string parameterName, object paramValue, Action<SqlParameter> configureParam = null);
        IStoredProcedure WithUserDefinedDataTableSqlParam<T>(string parameterName, IList<T> paramValue, Action<SqlParameter> configureParam = null);
    }
}
