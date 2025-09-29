namespace veterinaria.Models;

public abstract class Animal
{
    public int Id { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public double Weight { get; set; }
    public int Age { get; set; }
    public int ClientId { get; set; }
    public bool Diseased { get; set; }
    public string Description { get; set; }
    
    public Client Owner { get; set; }
}