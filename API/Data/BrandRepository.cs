using API.DTOs;
using API.DTOs.Brand;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class BrandRepository(DataContext context, IMapper mapper) : IBrandRepository
{
    public void Add(Brand brand) => context.Brands.Remove(brand);

    public void Delete(Brand brand) => context.Brands.Remove(brand);

    public async Task<bool> ExistsByIdAsync(int id) =>
        await context.Brands.AnyAsync(x => x.Id == id);

    public async Task<Brand?> GetAsNoTrackingByIdAsync(int id) =>
        await context.Brands
            .Include(x => x.VehicleBrands).ThenInclude(x => x.Vehicle)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id)
        ;

    public async Task<Brand?> GetByIdAsync(int id) =>
        await context.Brands
            .Include(x => x.VehicleBrands).ThenInclude(x => x.Vehicle)
            .SingleOrDefaultAsync(x => x.Id == id)
        ;

    public async Task<BrandDto?> GetDtoByIdAsync(int id) =>
        await context.Brands
            .Include(x => x.VehicleBrands).ThenInclude(x => x.Vehicle)
            .ProjectTo<BrandDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == id)
        ;

    public async Task<List<OptionDto>> GetOptionsAsync() =>
        await context.Brands
            .ProjectTo<OptionDto>(mapper.ConfigurationProvider)
            .ToListAsync()
        ;

    public async Task<PagedList<BrandDto>> GetPagedListAsync(BrandParams param)
    {
        IQueryable<Brand>? query = context.Brands
            .AsNoTracking()
            .AsQueryable()
        ;

        PagedList<BrandDto> pagedList = await PagedList<BrandDto>.CreateAsync(
            query.ProjectTo<BrandDto>(mapper.ConfigurationProvider), param.PageNumber, param.PageSize)
        ;

        return pagedList;
    }
}