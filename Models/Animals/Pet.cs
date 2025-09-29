using veterinaria.Models.Appointments;

namespace veterinaria.Models;

public class Pet : Animal
{
    public string PetName { get; set; }
    public Appointment Appointment { get; set; }
}