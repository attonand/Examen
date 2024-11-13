using API.Helpers;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API.DTOs.Vehicle;
using API.Interfaces;

namespace API.Controllers;

public class VehiclesController(IUnitOfWork uow, IMapper mapper, IVehiclesServices service) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<VehicleDto>>> GetPagedListAsync([FromQuery]VehicleParams param)
    {
        PagedList<VehicleDto> pagedList = await uow.VehicleRepository.GetPagedListAsync(param);
        
        Response.AddPaginationHeader(new PaginationHeader(
            pagedList.CurrentPage,
            pagedList.PageSize,
            pagedList.TotalCount,
            pagedList.TotalPages
        ));

        return pagedList;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleDto>> GetByIdAsync([FromRoute]int id)
    {
        VehicleDto? itemToReturn = await uow.VehicleRepository.GetDtoByIdAsync(id);

        if (itemToReturn == null) return NotFound($"El vehículo con ID {id} no fue encontrado.");

        return itemToReturn;
    }

    [HttpPost]
    public async Task<ActionResult<VehicleDto?>> CreateAsync([FromBody] VehicleCreateDto request)
    {
        if (request.Brand == null) return BadRequest("La marca del vehículo es requerida.");

        if (!request.Brand.Id.HasValue) return BadRequest("El ID de la marca no fue proporcionado.");

        int brandId = request.Brand.Id.Value;
        
        if (!await uow.BrandRepository.ExistsByIdAsync(brandId)) return NotFound($"La marca con ID {brandId} no fue encontrada.");
        
        // Brand? brand = await uow.BrandRepository.GetAsNoTrackingByIdAsync(brandId);

        // if (brand == null) return BadRequest($"No existe marca con ID {request.Brand}");

        Vehicle vehicleToCreate = new();

        vehicleToCreate.VehicleBrand = new(brandId);

        vehicleToCreate.Model = request.Model;
        vehicleToCreate.Year = request.Year;
        vehicleToCreate.Color = request.Color;

        if (request.Photos.Count == 0) return BadRequest("Debe agregar al menos una foto.");

        foreach(VehiclePhotoCreateDto photo in request.Photos) {
            if (string.IsNullOrWhiteSpace(photo.Url)) return BadRequest("Las fotos deben tener un URL");

            vehicleToCreate.VehiclePhotos.Add(new(photo.Url));
        }

        uow.VehicleRepository.Add(vehicleToCreate);
        
        if (!await uow.CompleteAsync()) return BadRequest("Errores al guardar el vehículo.");

        return await uow.VehicleRepository.GetDtoByIdAsync(vehicleToCreate.Id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VehicleDto>> UpdateAsync(int id, [FromBody] VehicleCreateDto request)
    {
        
        await Task.Delay(0);
        
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
    public async Task<ActionResult> DeleteByIdAsync([FromRoute] int id)
    {
        if (!await uow.VehicleRepository.ExistsByIdAsync(id)) return NotFound($"El vehículo con ID {id} no fue encontrado.");

        Vehicle? itemToDelete = await uow.VehicleRepository.GetByIdAsync(id);

        if (itemToDelete == null) return NotFound($"El vehículo con ID {id} no fue encontrado.");

        if (!await service.DeleteAsync(itemToDelete)) return BadRequest($"Error al eliminar el vehículo con ID {id}.");

        return Ok();
    }
}