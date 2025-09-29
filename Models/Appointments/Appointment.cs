namespace veterinaria.Models.Appointments;

public class Appointment
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int VetId { get; set; }
    public int? PetId { get; set; }
    public int? WildId { get; set; }
    
    public DateTime Date { get; set; }
    public string Diagnosis { get; set; }
    public string Medicaments { get; set; }
    
    public Client Client { get; set; }
    public Vet Vet { get; set; }
    public Pet Pet { get; set; }
    public Wild Wild { get; set; }
}