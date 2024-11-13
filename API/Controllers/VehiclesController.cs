using API.DataTransferObjects.Photo;
using API.DataTransferObjects.Vehicle;
using API.Helpers;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class VehiclesController(ApplicationDbContext context, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<VehicleSummaryResponse>>> GetPagedListAsync([FromQuery]string? term, [FromQuery]int? year, [FromQuery]int pageNumber = 1)
    {
        
            IQueryable<Vehicle>? query = context.Vehicles
                .Include(v => v.Brand)
                .AsNoTracking()
                .AsQueryable();

            if(!string.IsNullOrEmpty(term))
            {
                query = query.Where(v => v.Model.Contains(term));
                query = query.Where(v => v.Brand.Name.Contains(term));
                query = query.Where(v => v.Year.ToString() == term);
            }

            if(year.HasValue && year.Value > 0)
            {
                query = query.Where(v => v.Year == year.Value);
            } 



            PagedList<VehicleSummaryResponse>? pagedList = await PagedList<VehicleSummaryResponse>.CreateAsync(
                query.ProjectTo<VehicleSummaryResponse>(mapper.ConfigurationProvider),  
                pageNumber)
            ;

            if (pagedList == null)
            {
                return new PagedList<VehicleSummaryResponse>(new List<VehicleSummaryResponse>(), 0, 0, 0);
            }

            Response.AddPaginationHeader(new PaginationHeader(
                pagedList.CurrentPage,
                pagedList.PageSize,
                pagedList.TotalCount,
                pagedList.TotalPages
            ));

            return pagedList;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleDetailResponse>> Get(int id)
    {
        try
        {
            var vehicle = await context.Vehicles
                .Include(v => v.Brand)
                .Include(v => v.Photos)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var result = new VehicleDetailResponse
            {
                Model = vehicle.Model,
                BrandName = vehicle.Brand.Name,
                BrandId = vehicle.Brand.Id,
                Year = vehicle.Year,
                Color = vehicle.Color,
                
                Photos = vehicle.Photos.Select(p => new PhotoSummaryResponse { Id = p.Id, Url = p.URL }).ToList(),
            };
            
            return Ok(result);
        } catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<VehicleDetailResponse>> CreateAsync([FromBody] VehicleCreateRequest request)
    {
        
            Brand? brand = await context.Brands
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Name == request.Brand);

            if (brand == null) return BadRequest($"No existe marca con ID {request.Brand}");

            Vehicle vehicleToCreate = new();

            vehicleToCreate.BrandId = brand.Id;
            vehicleToCreate.Model = request.Model;
            vehicleToCreate.Year = request.Year;
            vehicleToCreate.Color = request.Color;

            if (request.Photos.Count == 0) return BadRequest("Debe agregar al menos una foto");

            foreach(VehiclePhotoCreateDto photo in request.Photos) {
                if (string.IsNullOrWhiteSpace(photo.Url)) return BadRequest("Las fotos deben tener un URL");

                vehicleToCreate.Photos.Add(new() { URL = photo.Url });
            }

            context.Vehicles.Add(vehicleToCreate);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = vehicleToCreate.Id }, vehicleToCreate);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VehicleDetailResponse>> Put(int id, [FromBody] VehicleCreateRequest request)
    {
        
        /*
            var brand = await context.Brands
                .FindAsync(request.Brand);

            if (brand == null)
            {
                return BadRequest($"No existe marca con ID {request.Brand}");
            }

            var vehicle = await context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var oldPhotos = await context.Photos
                .Where(p => p.VehicleId == id)
                .ToListAsync();
            context.Photos.RemoveRange(oldPhotos);

            vehicle.BrandId = request.Brand;
            vehicle.Model = request.Model;
            vehicle.Year = request.Year;
            vehicle.Color = request.Color;

            var newPhotos = request.PhotoURLs
                .Select(url => new Photo { VehicleId = vehicle.Id, URL = url }).ToList();
            context.Photos.AddRange(newPhotos);

            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();

            return NoContent();
            */

        return Ok();
        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var vehicle = await context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            context.Vehicles.Remove(vehicle);
            await context.SaveChangesAsync();

            return Ok();
        } catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

}