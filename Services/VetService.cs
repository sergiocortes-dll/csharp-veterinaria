using Microsoft.EntityFrameworkCore;
using veterinaria.Data;
using veterinaria.Models;

namespace veterinaria.Services;

public class VetService
{
    private readonly AppDbContext _context;

    public VetService(AppDbContext context)
    {
        _context = context;
    }
    
    public void AddVet(Vet vet)
    {
        _context.Vets.Add(vet);
        _context.SaveChanges();
    }

    public IEnumerable<Vet> GetAllVets()
    {
        return _context.Vets.Include(v => v.Appointments).ToList();
    }

    public Vet? GetVetById(int id)
    {
        return _context.Vets.Include(v => v.Appointments)
            .FirstOrDefault(v => v.Id == id);
    }

    public void UpdateVet(Vet vet)
    {
        _context.Vets.Update(vet);
        _context.SaveChanges();
    }

    public void DeleteVet(int id)
    {
        var vet = _context.Vets.Find(id);
        if (vet != null)
        {
            _context.Vets.Remove(vet);
            _context.SaveChanges();
        }
    }
    
    // Advanced LINQ
    
    public IEnumerable<Vet> GetSpecialistVets()
    {
        return _context.Vets
            .Where(v => v.Specialist)
            .Include(v => v.Appointments)
            .ToList();
    }
}