using System.Collections.Generic;
using System.Data;

namespace FluentStoredProcedureExtensions.Core.Abstract
{
    public interface ICollectionToDataTableConverter
    {
        DataTable ConvertToDataTable<T>(IList<T> entities);
    }
}
