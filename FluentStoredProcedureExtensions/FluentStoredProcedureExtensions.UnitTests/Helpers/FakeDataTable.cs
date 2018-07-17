using System.Data;
using System.Linq;

namespace FluentStoredProcedureExtensions.UnitTests.Helpers
{
    public static class FakeDataTable
    {
        public static DataTable GetDataTable()
        {
            var tableData = FakeCollectionData.GetTestData();
            var result = new DataTable();

            result.Columns.Add("Id", typeof(int));
            result.Columns.Add("Name", typeof(string));
            result.Rows.Add(tableData.First().Id, tableData.First().Name);
            result.Rows.Add(tableData.Last().Id, tableData.Last().Name);

            return result;
        }
    }
}
