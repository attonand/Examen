using API.Helpers;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API.DTOs.Brand;
using API.Interfaces;
using API.DTOs;

namespace API.Controllers;

public class BrandsController(IUnitOfWork uow, IMapper mapper, IBrandsServices service) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<BrandDto>>> GetPagedListAsync([FromQuery]BrandParams param)
    {
        PagedList<BrandDto> pagedList = await uow.BrandRepository.GetPagedListAsync(param);
        
        Response.AddPaginationHeader(new PaginationHeader(
            pagedList.CurrentPage,
            pagedList.PageSize,
            pagedList.TotalCount,
            pagedList.TotalPages
        ));

        return pagedList;
    }

    [HttpGet("options")]
    public async Task<ActionResult<List<OptionDto>>> GetOptionsAsync() =>
        await uow.BrandRepository.GetOptionsAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<BrandDto>> GetByIdAsync([FromRoute]int id)
    {
        BrandDto? itemToReturn = await uow.BrandRepository.GetDtoByIdAsync(id);

        if (itemToReturn == null) return NotFound($"La marca con ID {id} no fue encontrado.");

        return itemToReturn;
    }

    [HttpPost]
    public async Task<ActionResult<BrandDto?>> CreateAsync([FromBody] BrandCreateDto request)
    {
        Brand brandToCreate = new();

        brandToCreate.Name = request.Name;

        uow.BrandRepository.Add(brandToCreate);
        
        if (!await uow.CompleteAsync()) return BadRequest("Errores al guardar la marca.");

        return await uow.BrandRepository.GetDtoByIdAsync(brandToCreate.Id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BrandDto>> UpdateAsync(int id, [FromBody] BrandCreateDto request)
    {
        
        await Task.Delay(0);
        
        /*
            var brand = await context.Brands
                .FindAsync(request.Brand);

            if (brand == null)
            {
                return BadRequest($"No existe marca con ID {request.Brand}");
            }

            var brand = await context.Brands.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            var oldPhotos = await context.Photos
                .Where(p => p.BrandId == id)
                .ToListAsync();
            context.Photos.RemoveRange(oldPhotos);

            brand.BrandId = request.Brand;
            brand.Model = request.Model;
            brand.Year = request.Year;
            brand.Color = request.Color;

            var newPhotos = request.PhotoURLs
                .Select(url => new Photo { BrandId = brand.Id, URL = url }).ToList();
            context.Photos.AddRange(newPhotos);

            context.Brands.Add(brand);
            await context.SaveChangesAsync();

            return NoContent();
            */

        return Ok();
        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteByIdAsync([FromRoute] int id)
    {
        if (!await uow.BrandRepository.ExistsByIdAsync(id)) return NotFound($"La marca con ID {id} no fue encontrado.");

        Brand? itemToDelete = await uow.BrandRepository.GetByIdAsync(id);

        if (itemToDelete == null) return NotFound($"La marca con ID {id} no fue encontrado.");

        if (!await service.DeleteAsync(itemToDelete)) return BadRequest($"Error al eliminar la marca con ID {id}.");

        return Ok();
    }
}