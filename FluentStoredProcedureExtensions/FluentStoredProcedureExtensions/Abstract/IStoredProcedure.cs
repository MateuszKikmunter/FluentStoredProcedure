using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FluentStoredProcedureExtensions.Core.Abstract
{
    public interface IStoredProcedure
    {
        string StoredProcedureText { get; }
        void SetStoredProcedureText(string commandText);
        IList<SqlParameter> SqlParametersCollection { get; }
        IStoredProcedure WithSqlParam(string parameterName, Action<SqlParameter> configureParameter = null);
        IStoredProcedure WithSqlParam(string parameterName, object paramValue, Action<SqlParameter> configureParam = null);
        IStoredProcedure WithUserDefinedDataTableSqlParam<T>(string parameterName, IList<T> paramValue, Action<SqlParameter> configureParam = null);
    }
}
