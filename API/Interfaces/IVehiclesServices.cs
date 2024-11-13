using API.Entities;

namespace API.Interfaces;

public interface IVehiclesServices {
    Task<bool> DeleteAsync(Vehicle item);
}
