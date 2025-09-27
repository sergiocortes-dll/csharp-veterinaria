using Microsoft.EntityFrameworkCore;
using veterinaria.Models;

namespace veterinaria.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vet> Vets { get; set; }
    public DbSet<Client> Clients { get; set; }
}