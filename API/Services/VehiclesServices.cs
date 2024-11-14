using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class VehiclesServices(DataContext context) : IVehiclesServices
{
    public async Task<bool> DeleteAsync(Vehicle item) {


        Vehicle? itemAsNoTracking = await context.Vehicles
            .Include(x => x.VehicleBrand.Brand)
            .Include(x => x.VehiclePhotos)
                .ThenInclude(x => x.Photo)
            .AsNoTracking()
            .Where(x => x.Id == item.Id)
            .SingleOrDefaultAsync();

        if (itemAsNoTracking == null) return true;
            if (!await DeleteVehiclePhotosAsync(itemAsNoTracking)) return false;
            //if (!await DeleteVehicleBrandAsync(itemAsNoTracking)) return false;

            context.Vehicles.Remove(item);

            return await context.SaveChangesAsync() > 0;
    }

    private async Task<bool> DeleteVehiclePhotosAsync(Vehicle item) {
        if (item.VehiclePhotos.Count() == 0) return true;
        
        foreach(VehiclePhoto vehiclePhoto in item.VehiclePhotos) {
            Photo? photoToDelete = await context.Photos
                .SingleOrDefaultAsync(x => x.Id == vehiclePhoto.PhotoId)
            ;

            if (photoToDelete != null) {
                context.Photos.Remove(photoToDelete);
                
                if (await context.SaveChangesAsync() <= 0) return false;
            }
        }
        
        return true;
    }

    private async Task<bool> DeleteVehicleBrandAsync(Vehicle item) {    
        if (item.VehicleBrand == null) return true;
        
        Brand? brandToDelete = await context.Brands
                .SingleOrDefaultAsync(x => x.Id == item.VehicleBrand.BrandId)
            ;

        if(brandToDelete != null) {
            context.Brands.Remove(brandToDelete);
            
            if(await context.SaveChangesAsync() <= 0) return false;
        }

        return true;
    }
}
