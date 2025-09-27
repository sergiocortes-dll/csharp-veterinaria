namespace veterinaria.Models;

public abstract class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
}