using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace FluentStoredProcedure.IntegrationTests.Helpers
{
    internal static class SqlScriptRunner
    {
        private static string _sqlFileExtension => "*.sql";
        private static string _sqlScriptsFolderName => "SqlScripts";
        private static string _sqlScriptsLocation => AppDomain.CurrentDomain.BaseDirectory + _sqlScriptsFolderName;

        /// <summary>
        /// Removes tables, types and stored procedures from test database.
        /// </summary>
        public static void ClearDatabase()
        {
            ExecuteScript("ClearDatabase");
        }

        /// <summary>
        /// Creates tables, types and stored procedures required for integration testing.
        /// </summary>
        public static void SetUpDatabase()
        {
            ExecuteScript("SetUpDatabase");
        }

        private static IEnumerable<string> GetFileNames()
        {
            return Directory
                .GetFiles(_sqlScriptsLocation, _sqlFileExtension)
                .Select(Path.GetFileName);
        }

        private static string GetScriptToRunByName(string scriptName)
        {
            var script = GetFileNames().First(fileName => fileName.Contains(scriptName));
            return $"{_sqlScriptsLocation}\\{script}";
        }

        private static string[] GetSqlFromFile(string scriptName)
        {
            var splitter = new[] { "\r\nGO\r\n" };
            var fileContent = File.ReadAllText(GetScriptToRunByName(scriptName));
            return fileContent.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
        }

        private static void ExecuteScript(string scriptName)
        {
            var sql = GetSqlFromFile(scriptName);
            using (var connection = new SqlConnection(ConnectionStringProvider.GetConnectionString()))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                foreach (var sqlStatement in sql)
                {
                    cmd.CommandText = sqlStatement;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
