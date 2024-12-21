using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DotNetEnv;

namespace MyESGIApi.Data
{
    public class DatabaseHelper
    {
        private static string? ConnectionString;
        private static readonly ILogger<DatabaseHelper> _logger;

        static DatabaseHelper()
        {
            Env.Load();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var serviceProvider = new ServiceCollection()
                .AddLogging(config => config.AddConsole())
                .BuildServiceProvider();

            _logger = serviceProvider
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger<DatabaseHelper>();
            ConnectionString = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");

            if (ConnectionString == null)
            {
                _logger.LogWarning("Environment variable DEFAULT_CONNECTION not found, falling back to appsettings.json");
                ConnectionString = configuration.GetConnectionString("DefaultConnection");
            }
            else if (ConnectionString != null)
            {
                _logger.LogInformation("Database connection string initialized.");
            }
            else
            {
                _logger.LogError("Database connection string is not set.");
            }
        }

        private static string? GetConnectionString()
        {
            return ConnectionString != null ? ConnectionString : throw new Exception("Database connection string is not set.");
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}
