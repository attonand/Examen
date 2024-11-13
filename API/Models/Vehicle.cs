namespace API.Models;

public class Vehicle
{
    public int Id { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Year { get; set; }

    public int BrandId { get; set; }
    public Brand Brand { get; set; } = new Brand();

    public virtual ICollection<Photo> Photos { get; set; } = [];
}