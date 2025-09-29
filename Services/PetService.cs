using Microsoft.EntityFrameworkCore;
using veterinaria.Data;
using veterinaria.Models;

namespace veterinaria.Services;

public class PetService
{
    private readonly AppDbContext _context;

    public PetService(AppDbContext context)
    {
        _context = context;
    }

    public void AddPet(Pet pet)
    {
        _context.Pets.Add(pet);
        _context.SaveChanges();
    }

    public IEnumerable<Pet> GetAllPets()
    {
        return _context.Pets.Include(p => p.Owner).ToList();
    }

    public Pet? GetPetById(int id)
    {
        return _context.Pets
            .Include(p => p.Owner)
            .FirstOrDefault(p => p.Id == id);
    }

    public void UpdatePet(Pet pet)
    {
        _context.Pets.Update(pet);
        _context.SaveChanges();
    }

    public void DeletePet(int id)
    {
        var pet = _context.Pets.Find(id);
        if (pet != null)
        {
            _context.Pets.Remove(pet);
            _context.SaveChanges();
        }
    }

    // Advanced LINQ

    public IEnumerable<Pet> GetPetsByClient(int clientId)
    {
        return _context.Pets.Where(p => p.ClientId == clientId).ToList();
    }

    public Client? GetClientWithMostPets()
    {
        return _context.Clients
            .Select(c => new { Client = c, PetCount = c.Pets.Count })
            .OrderByDescending(x => x.PetCount)
            .Select(x => x.Client)
            .FirstOrDefault();
    }
}