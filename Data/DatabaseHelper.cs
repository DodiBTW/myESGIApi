using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace myESGIApi.Data
{
    public class DatabaseHelper
    {
        private static string? ConnectionString;
        private static readonly ILogger<DatabaseHelper> _logger;

        static DatabaseHelper()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var serviceProvider = new ServiceCollection()
                .AddLogging(config => config.AddConsole())
                .BuildServiceProvider();

            _logger = serviceProvider
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger<DatabaseHelper>();
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
            _logger.LogInformation("Initializing DatabaseHelper");
            _logger.LogInformation(AppContext.BaseDirectory);
        }

        private static string GetConnectionString()
        {
            if(ConnectionString != null)
            {
                return ConnectionString;
            }
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            ConnectionString = configuration.GetConnectionString("DefaultConnection");
            return ConnectionString;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}
