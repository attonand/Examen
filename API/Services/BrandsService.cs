using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class BrandsServices(DataContext context) : IBrandsServices
{
    public async Task<bool> DeleteAsync(Brand item) {
        Brand? itemAsNoTracking = await context.Brands
            .Include(x => x.VehicleBrands).ThenInclude(x => x.Vehicle)
            .SingleOrDefaultAsync(x => x.Id == item.Id);

        if (itemAsNoTracking == null) return true;

        context.Brands.Remove(item);

        return await context.SaveChangesAsync() > 0;
    }
}
