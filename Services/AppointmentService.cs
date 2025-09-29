using Microsoft.EntityFrameworkCore;
using veterinaria.Data;
using veterinaria.Models;
using veterinaria.Models.Appointments;

namespace veterinaria.Services;

public class AppointmentService
{
    private readonly AppDbContext _context;

    public AppointmentService(AppDbContext context)
    {
        _context = context;
    }
    
    public void AddAppointment(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        _context.SaveChanges();
    }

    public IEnumerable<Appointment> GetAllAppointments()
    {
        return _context.Appointments
            .Include(a => a.Client)
            .Include(a => a.Vet)
            .Include(a => a.Pet)
            .Include(a => a.Wild)
            .ToList();
    }

    public Appointment? GetAppointmentById(int id)
    {
        return _context.Appointments
            .Include(a => a.Client)
            .Include(a => a.Vet)
            .Include(a => a.Pet)
            .Include(a => a.Wild)
            .FirstOrDefault(a => a.Id == id);
    }

    public void UpdateAppointment(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        _context.SaveChanges();
    }

    public void DeleteAppointment(int id)
    {
        var appointment = _context.Appointments.Find(id);
        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
        }
    }
    
    // Advanced LINQ

    public IEnumerable<Appointment> GetAppointmentsByPet(int petId)
    {
        return _context.Appointments
            .Where(a => a.PetId == petId)
            .Include(a => a.Client)
            .Include(a => a.Vet)
            .Include(a => a.Pet)
            .OrderByDescending(a => a.Date)
            .ToList();
    }

    public Vet? GetVetWithMostAppointments()
    {
        return GetVetWithMostAppointments(DateTime.MinValue, DateTime.MaxValue);
    }

    public Vet? GetVetWithMostAppointments(DateTime startDate, DateTime endDate)
    {
        return _context.Vets
            .Select(v => new 
            { 
                Vet = v, 
                Count = v.Appointments.Count(a => a.Date >= startDate && a.Date <= endDate)
            })
            .Where(x => x.Count > 0)
            .OrderByDescending(x => x.Count)
            .Select(x => x.Vet)
            .FirstOrDefault();
    }

    public string? GetMostAttendedSpecies()
    {
        var speciesCount = _context.Appointments
            .Where(a => a.PetId != null)
            .Join(_context.Pets,
                appointment => appointment.PetId.Value,
                pet => pet.Id,
                (appointment, pet) => pet.Species)
            .GroupBy(species => species)
            .Select(group => new { Species = group.Key, Count = group.Count() })
            .OrderByDescending(x => x.Count)
            .FirstOrDefault();

        return speciesCount?.Species;
    }
}