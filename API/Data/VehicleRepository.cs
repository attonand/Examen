using API.DTOs.Vehicle;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class VehicleRepository(DataContext context, IMapper mapper) : IVehicleRepository
{
    public void Add(Vehicle vehicle) => context.Vehicles.Add(vehicle);

    public void Delete(Vehicle vehicle) => context.Vehicles.Remove(vehicle);

    public void Update(Vehicle vehicle) => context.Vehicles.Update(vehicle);

    public async Task<bool> ExistsByIdAsync(int id) =>
        await context.Vehicles.AnyAsync(x => x.Id == id);

    public async Task<Vehicle?> GetAsNoTrackingByIdAsync(int id) =>
        await context.Vehicles
            .Include(x => x.VehiclePhotos).ThenInclude(x => x.Photo)
            .Include(x => x.VehicleBrand.Brand)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id)
        ;

    public async Task<Vehicle?> GetByIdAsync(int id) =>
        await context.Vehicles
            .Include(x => x.VehiclePhotos).ThenInclude(x => x.Photo)
            .Include(x => x.VehicleBrand.Brand)
            .SingleOrDefaultAsync(x => x.Id == id)
        ;

    public async Task<VehicleDto?> GetDtoByIdAsync(int id) =>
        await context.Vehicles
            .Include(x => x.VehiclePhotos).ThenInclude(x => x.Photo)
            .Include(x => x.VehicleBrand.Brand)
            .ProjectTo<VehicleDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == id)
        ;

    public async Task<PagedList<VehicleDto>> GetPagedListAsync(VehicleParams param)
    {
        IQueryable<Vehicle>? query = context.Vehicles
            .Include(v => v.VehicleBrand.Brand)
            .Include(x => x.VehiclePhotos).ThenInclude(x => x.Photo)
            .AsNoTracking()
            .AsQueryable()
        ;

        if(!string.IsNullOrEmpty(param.Term))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.Model) && x.Model == param.Term);
            query = query.Where(x => x.Year.HasValue && x.Year.ToString() == param.Term);
            query = query.Where(x => x.VehicleBrand != null &&
                x.VehicleBrand.Brand != null &&
                !string.IsNullOrEmpty(x.VehicleBrand.Brand.Name) &&
                x.VehicleBrand.Brand.Name == param.Term
            );
            query = query.Where(x => !string.IsNullOrEmpty(x.Model) && x.Model == param.Term);
        }

        if(param.Year.HasValue && param.Year.Value > 0) {
            query = query.Where(v => v.Year == param.Year.Value);
        } 

        PagedList<VehicleDto> pagedList = await PagedList<VehicleDto>.CreateAsync(
            query.ProjectTo<VehicleDto>(mapper.ConfigurationProvider), param.PageNumber, param.PageSize)
        ;

        return pagedList;
    }
}