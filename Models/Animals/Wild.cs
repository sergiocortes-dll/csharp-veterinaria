using veterinaria.Models.Appointments;

namespace veterinaria.Models;

public class Wild : Animal
{
    public string NativeOf { get; set; }
    public Appointment Appointment { get; set; }
}