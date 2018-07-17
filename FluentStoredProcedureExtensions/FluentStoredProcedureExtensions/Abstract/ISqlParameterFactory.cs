using System.Collections.Generic;
using System.Data.SqlClient;

namespace FluentStoredProcedureExtensions.Core.Abstract
{
    public interface ISqlParameterFactory
    {
        SqlParameter CreateParameter(string paramName, object param);
        SqlParameter CreateParameter(string paramName);
        SqlParameter BuildUserDefinedTableTypeParameter<T>(string paramName, IList<T> value);
    }
}
