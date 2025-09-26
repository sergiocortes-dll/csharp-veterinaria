using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql;
using veterinaria.Data;
using veterinaria.Services;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Build configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Validate connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("ERROR: Connection string 'DefaultConnection' not found in appsettings.json");
                return;
            }
            
            // Print connection string (hide password for security)
            var safeConnectionString = connectionString
                .Replace("Pwd=", "Pwd=***")
                .Replace("Password=", "Password=***");
            Console.WriteLine($"Using connection string: {safeConnectionString}");

            // Set up dependency injection
            var services = new ServiceCollection();
            
            // Add logging
            services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information));
            
            // Register IConfiguration
            services.AddSingleton(configuration);
            
            // Configure DbContext with retry logic and SSL for cloud database
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 36)),
                    mySqlOptions =>
                    {
                        mySqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        mySqlOptions.CommandTimeout(60); // 60 seconds timeout
                    })
                .EnableSensitiveDataLogging() // Only for debugging
                .EnableDetailedErrors());

            services.AddScoped<ClientService>();

            // Build service provider
            var serviceProvider = services.BuildServiceProvider();

            // Test database connection and operations
            await TestDatabaseOperations(serviceProvider);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Application failed to start: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }

    static async Task TestDatabaseOperations(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        try
        {
            // Test database connection
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            logger.LogInformation("Testing database connection...");
            
            try
            {
                var canConnect = await context.Database.CanConnectAsync();
                
                if (!canConnect)
                {
                    logger.LogError("Cannot connect to database - CanConnectAsync returned false");
                    return;
                }
            }
            catch (Exception connEx)
            {
                logger.LogError($"Database connection failed: {connEx.Message}");
                if (connEx.InnerException != null)
                {
                    logger.LogError($"Inner exception: {connEx.InnerException.Message}");
                }
                logger.LogError($"Stack trace: {connEx.StackTrace}");
                return;
            }
            
            logger.LogInformation("Database connection successful!");
            
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();
            logger.LogInformation("Database schema verified/created");

            // Test ClientService
            var clientService = scope.ServiceProvider.GetRequiredService<ClientService>();
            
            logger.LogInformation("Retrieving clients...");
            var clients = clientService.GetClients();
            
            if (!clients.Any())
            {
                logger.LogInformation("No clients found in database");
            }
            else
            {
                foreach (var client in clients)
                {
                    Console.WriteLine($"Cliente: {client.Name}");
                }
            }
        }
        catch (MySqlException mysqlEx)
        {
            Console.WriteLine($"MySQL Error: {mysqlEx.Message}");
            Console.WriteLine($"Error Number: {mysqlEx.Number}");
            Console.WriteLine($"SQL State: {mysqlEx.SqlState}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
        }
    }
}