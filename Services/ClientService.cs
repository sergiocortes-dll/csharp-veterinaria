using Microsoft.EntityFrameworkCore;
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

    // Create client
    public void AddClient(Client client)
    {
        _context.Clients.Add(client);
        _context.SaveChanges();
    }

    // Enum all
    public IEnumerable<Client> GetAllClients()
    {
        return _context.Clients
            .Include(c => c.Pets)
            .Include(c => c.Wilds)
            .Include(c => c.Appointments)
            .ToList();
    }
    
    // Search by ID
    public Client? GetClientById(int id)
    {
        return _context.Clients
            .Include(c => c.Pets)
            .Include(c => c.Wilds)
            .FirstOrDefault(c => c.Id == id);
    }
    
    // Edit Client
    public void UpdateClient(Client client)
    {
        _context.Clients.Update(client);
        _context.SaveChanges();
    }
    
    // Delete Client
    public void DeleteClient(int id)
    {
        var client = _context.Clients.Find(id);
        if (client != null)
        {
            _context.Clients.Remove(client);
            _context.SaveChanges();
        }
    }
}