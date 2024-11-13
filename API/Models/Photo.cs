namespace API.Models;

public class Photo
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public string URL { get; set; } = string.Empty;
}