using API.DataTransferObjects.Photo;

namespace API.DataTransferObjects.Vehicle;

public class VehicleSummaryResponse
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;

    public List<PhotoSummaryResponse> Photos { get; set; } = []; 
}