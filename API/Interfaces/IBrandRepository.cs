using API.DTOs;
using API.DTOs.Brand;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IBrandRepository {
    Task<PagedList<BrandDto>> GetPagedListAsync(BrandParams param);
    Task<Brand?> GetByIdAsync(int id);
    Task<Brand?> GetAsNoTrackingByIdAsync(int id);
    Task<BrandDto?> GetDtoByIdAsync(int id);
    void Delete(Brand vehicle);
    void Add(Brand vehicle);
    Task<bool> ExistsByIdAsync(int id);
    Task<List<OptionDto>> GetOptionsAsync();
}
