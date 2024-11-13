using API.DataTransferObjects.Brand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[AllowAnonymous]
public class BrandsController(ApplicationDbContext context) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<BrandSummary>>> Get()
    {
        var brands = await context.Brands
                .AsNoTracking()
                .ToListAsync();

            var result = brands.Select(b => new BrandSummary {
                Id = b.Id,
                Name = b.Name
            });

            return Ok(result); 
    }
}