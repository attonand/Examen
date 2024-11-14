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
    public async Task<ActionResult<VehicleDto?>> UpdateAsync(int id, [FromBody] VehicleCreateDto request)
    {
        if (request.Brand == null) return BadRequest("La marca del vehículo es requerida.");

        if (!request.Brand.Id.HasValue) return BadRequest("El ID de la marca no fue proporcionado.");

        int brandId = request.Brand.Id.Value;

        if (!await uow.BrandRepository.ExistsByIdAsync(brandId)) return NotFound($"La marca con ID {brandId} no fue encontrada.");
        
        Vehicle? itemToUpdate = await uow.VehicleRepository.GetByIdAsync(id);

        if (itemToUpdate == null) return NotFound($"El vehículo con ID {id} no fue encontrado.");        

        foreach(VehiclePhoto photoToRemove in itemToUpdate.VehiclePhotos) {
            uow.PhotoRepository.Delete(photoToRemove.Photo);
        }

        itemToUpdate.VehicleBrand = new(brandId);
        itemToUpdate.Color = request.Color;
        itemToUpdate.Year = request.Year;
        itemToUpdate.Model = request.Model;

        foreach(VehiclePhotoCreateDto photo in request.Photos) {
            if (string.IsNullOrWhiteSpace(photo.Url)) return BadRequest("Las fotos deben tener un URL");

            itemToUpdate.VehiclePhotos.Add(new(photo.Url));
        }

        uow.VehicleRepository.Update(itemToUpdate);
        
        if (!await uow.CompleteAsync()) return BadRequest("Errores al guardar el vehículo.");

        return await uow.VehicleRepository.GetDtoByIdAsync(itemToUpdate.Id);        
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