namespace API.Helpers;

public class PhotoParams : PaginationParams {
    public string? Term { get; set; } = null;
    public int? Year { get; set; } = null;
}