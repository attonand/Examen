using API.DataTransferObjects.Photo;

namespace API.DataTransferObjects.Vehicle;

public class VehicleDetailResponse
{
    public int Id { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public int BrandId { get; set; }
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;

    public List<PhotoSummaryResponse> Photos { get; set; } = []; 
}