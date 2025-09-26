using veterinaria.Data;
using veterinaria.Models;

namespace veterinaria.Services;

public class ClientService
{
    private readonly AppDbContext _context;

    public ClientService(AppDbContext context)
    {
        _context = context;
    }

    public void InsertClient()
    {
        var client = new Client
        {

        };
        _context.Clients.Add(client);
        _context.SaveChanges();
    }

    public IEnumerable<Client> GetClients()
    {
        return _context.Clients.ToList();
    }
}