using API.Entities;

namespace API.Interfaces;

public interface IBrandsServices {
    Task<bool> DeleteAsync(Brand item);
}
