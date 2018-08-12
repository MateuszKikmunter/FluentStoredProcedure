using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Infrastructure.Services;

namespace FluentStoredProcedureExtensions.Infrastructure.Data
{
    public class CollectionToDataTableConverter : ICollectionToDataTableConverter
    {
        public DataTable ConvertToDataTable<T>(IList<T> entities)
        {
            Guard.ThrowIfCollectionNullOrEmpty(entities);

            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in entities)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
