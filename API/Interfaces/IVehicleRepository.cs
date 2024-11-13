using API.DTOs.Vehicle;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IVehicleRepository {
    Task<PagedList<VehicleDto>> GetPagedListAsync(VehicleParams param);
    Task<Vehicle?> GetByIdAsync(int id);
    Task<Vehicle?> GetAsNoTrackingByIdAsync(int id);
    Task<VehicleDto?> GetDtoByIdAsync(int id);
    void Delete(Vehicle vehicle);
    void Add(Vehicle vehicle);
    Task<bool> ExistsByIdAsync(int id);
}
