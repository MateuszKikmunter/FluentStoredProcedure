using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FluentStoredProcedureExtensions.Infrastructure.Extensions
{
    public static class MigrationBuilderExtensions
    {
        /// <summary>
        /// Runs SQL from specified file.
        /// File has to be in the DataAccess/SqlScripts folder.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static MigrationBuilder RunFile(this MigrationBuilder builder, string filename)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var sqlPath = $"{AppDomain.CurrentDomain.BaseDirectory}DataAccess\\SqlScripts\\{filename}.sql";
            if (File.Exists(sqlPath))
            {
                builder.Sql(File.ReadAllText(sqlPath));
            }
            else
            {
                throw new Exception($"Migration .sql file not found: { filename }.");
            }

            return builder;
        }
        
        /// <summary>
        /// Works only with dbo schema
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="storedProcedureName"></param>
        /// <returns></returns>
        public static MigrationBuilder DropStoredProcedureIfExists(this MigrationBuilder builder, string storedProcedureName)
        {
            builder.Sql(
                $@"IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('dbo.{storedProcedureName}')) DROP PROCEDURE [dbo].[{storedProcedureName}] GO");
            return builder;
        }
    }
}
