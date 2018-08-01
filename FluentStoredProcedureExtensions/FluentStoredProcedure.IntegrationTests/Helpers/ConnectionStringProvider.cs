using System;
using Microsoft.Extensions.Configuration;

namespace FluentStoredProcedure.IntegrationTests.Helpers
{
    public static class ConnectionStringProvider
    {
        public static string GetConnectionString()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", true, true)
                .Build()
                .GetConnectionString("FluentStoredProcedure");
        }
    }
}
