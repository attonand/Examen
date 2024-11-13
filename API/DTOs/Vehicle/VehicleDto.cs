using API.DTOs.Brand;
using API.DTOs.Photo;

namespace API.DTOs.Vehicle;

public class VehicleDto
{
    public int Id { get; set; }
    public string? Model { get; set; } = null;
    public string? Color { get; set; } = null;
    public int? Year { get; set; } = null;

    public OptionDto? Brand { get; set; } = null;


    public List<PhotoDto> Photos { get; set; } = []; 
}