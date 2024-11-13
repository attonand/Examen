namespace API.Helpers;

public class VehicleParams : PaginationParams {
    public string? Term { get; set; } = null;
    public int? Year { get; set; } = null;
}