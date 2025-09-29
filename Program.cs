using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using veterinaria;
using veterinaria.Data;
using veterinaria.Services;
using veterinaria.UI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices((hostContext, services) =>
        {
            var configuration = hostContext.Configuration;
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                // Use temporal ServiceProvider to log the error
                using var tempProvider = services.BuildServiceProvider();
                var logger = tempProvider.GetService<ILogger<Program>>();
                logger?.LogError("ERROR: Connection string 'DefaultConnection' is missing");
                Environment.Exit(1);
            }

            // ---- DbContext con MySQL (Pomelo) ----
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(8, 0, 36)),
                    mySqlOptions =>
                    {
                        mySqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        mySqlOptions.CommandTimeout(60);
                    })
                .EnableSensitiveDataLogging(hostContext.HostingEnvironment.IsDevelopment())
                .EnableDetailedErrors());

            // ---- Services ----
            services.AddScoped<ClientService>();
            services.AddScoped<PetService>();
            services.AddScoped<VetService>();
            services.AddScoped<AppointmentService>();

            // ---- UI ----
            services.AddTransient<Menu>();

            // ---- AppRunner ----
            services.AddTransient<AppRunner>();
        });

        var host = builder.Build();

        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Iniciando la aplicación...");
            var runner = services.GetRequiredService<AppRunner>();
            await runner.RunAsync();
            logger.LogInformation("La aplicación finalizó correctamente.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Ocurrió un error fatal en la aplicación.");
        }
    }
}
