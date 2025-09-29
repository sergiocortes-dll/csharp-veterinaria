using veterinaria.Models.Appointments;

namespace veterinaria.Models;

public class Vet : Person
{
    public bool Specialist { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}