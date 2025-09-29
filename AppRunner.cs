using Microsoft.Extensions.Logging;
using MySqlConnector;
using veterinaria.Data;
using veterinaria.Services;
using veterinaria.UI;

namespace veterinaria;

public class AppRunner
{
    private readonly Menu _menu;
    private readonly ILogger<AppRunner> _logger;
    private readonly ClientService _clientService;
    private readonly AppDbContext _context;
    
    // Inject services through constructor
    public AppRunner(Menu menu, ILogger<AppRunner> logger, ClientService clientService, AppDbContext context)
    {
        _menu = menu;
        _logger = logger;
        _clientService = clientService;
        _context = context;
    }

    public async Task RunAsync()
    {
        _logger.LogInformation("Mostrando menú principal...");
        await Task.Run(() => _menu.Show());
    }

    public async Task RunTestAsync()
    {
        try
        {
            _logger.LogInformation("Verificando la conexión con la base de datos...");

            // CanConnectAsync to test connection
            if (!await _context.Database.CanConnectAsync())
            {
                _logger.LogError("No se pudo conectar a la base de datos.");
                return;
            }

            _logger.LogInformation("¡Conexión exitosa!");

            _logger.LogInformation("Obteniendo la lista de clientes...");
            var clients = _clientService.GetAllClients();

            if (!clients.Any())
            {
                _logger.LogWarning("No se encontraron los clientes en la base de datos.");
            }
            else
            {
                _logger.LogInformation("-- Lista de Clientes --");
                foreach (var client in clients)
                {
                    _logger.LogInformation($"ID: {client.Id}");
                }
            }
        }
        catch (MySqlException mysqlEx)
        {
            _logger.LogError(mysqlEx, "Error de MySQL (Número: {ErrorNumber}): {ErrorMessage}", mysqlEx.Number,
                mysqlEx.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error inesperado durante la ejecución.");
        }
    }
}