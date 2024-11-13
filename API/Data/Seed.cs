using API.Entities;

namespace API.Data;

public static class Seed
{
    public static async Task SeedAsync(DataContext context)
    {

        var brands = new List<Brand>
        {
            new Brand { Id = 1, Name = "Toyota" },
            new Brand { Id = 2, Name = "Mercedes-Benz" },
            new Brand { Id = 3, Name = "BMW" },
            new Brand { Id = 4, Name = "Volkswagen" },
            new Brand { Id = 5, Name = "Honda" },
            new Brand { Id = 6, Name = "Ford" },
            new Brand { Id = 7, Name = "Chevrolet" },
            new Brand { Id = 8, Name = "Nissan" },
            new Brand { Id = 9, Name = "Audi" },
            new Brand { Id = 10, Name = "Hyundai" }
        };

        if(!context.Brands.Any())
        {
            context.Brands.AddRange(brands);
            await context.SaveChangesAsync();
        }
    }
}