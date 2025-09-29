using veterinaria.Models.Appointments;

namespace veterinaria.Models;

public class Client : Person
{
    public bool Insurance { get; set; }

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
    public ICollection<Wild> Wilds { get; set; } = new List<Wild>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}