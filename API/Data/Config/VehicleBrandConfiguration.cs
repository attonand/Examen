using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Config;

public class VehicleBrandConfiguration : IEntityTypeConfiguration<VehicleBrand>
{
    public void Configure(EntityTypeBuilder<VehicleBrand> builder)
    {
        builder.HasKey(x => new { x.VehicleId, x.BrandId});

        builder
            .HasOne(x => x.Vehicle)
            .WithOne(x => x.VehicleBrand)
            .HasForeignKey<VehicleBrand>(x => x.VehicleId)
            .OnDelete(DeleteBehavior.NoAction)
        ;

        builder
            .HasOne(x => x.Brand)
            .WithMany(x => x.VehicleBrands)
            .HasForeignKey(x => x.BrandId)
            .OnDelete(DeleteBehavior.NoAction)
        ;
    }
}
