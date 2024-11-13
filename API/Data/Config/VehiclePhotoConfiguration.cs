using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Config;

public class VehiclePhotoConfiguration : IEntityTypeConfiguration<VehiclePhoto>
{
    public void Configure(EntityTypeBuilder<VehiclePhoto> builder)
    {
        builder.HasKey(x => new { x.VehicleId, x.PhotoId});

        builder
            .HasOne(x => x.Vehicle)
            .WithMany(x => x.VehiclePhotos)
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.NoAction)
        ;

        builder
            .HasOne(x => x.Photo)
            .WithOne(x => x.VehiclePhoto)
            .HasForeignKey<VehiclePhoto>(x => x.PhotoId)
            .OnDelete(DeleteBehavior.Cascade)
        ;
    }
}
