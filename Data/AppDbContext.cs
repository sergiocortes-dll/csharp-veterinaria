using Microsoft.EntityFrameworkCore;
using veterinaria.Models;
using veterinaria.Models.Appointments;

namespace veterinaria.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Vet> Vets { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Wild> Wilds { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Relaciones 1-1 entre Appointment y Pet/Wild
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Pet)
            .WithOne(p => p.Appointment)
            .HasForeignKey<Appointment>(a => a.PetId);
        
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Wild)
            .WithOne(w => w.Appointment)
            .HasForeignKey<Appointment>(a => a.WildId);
    }
}