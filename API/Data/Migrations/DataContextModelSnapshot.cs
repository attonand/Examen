﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("API.Entities.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("API.Entities.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("API.Entities.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .HasColumnType("TEXT");

                    b.Property<string>("Model")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("API.Entities.VehicleBrand", b =>
                {
                    b.Property<int>("VehicleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BrandId")
                        .HasColumnType("INTEGER");

                    b.HasKey("VehicleId", "BrandId");

                    b.HasIndex("BrandId");

                    b.HasIndex("VehicleId")
                        .IsUnique();

                    b.ToTable("VehicleBrand");
                });

            modelBuilder.Entity("API.Entities.VehiclePhoto", b =>
                {
                    b.Property<int>("VehicleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PhotoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("VehicleId", "PhotoId");

                    b.HasIndex("PhotoId")
                        .IsUnique();

                    b.ToTable("VehiclePhoto");
                });

            modelBuilder.Entity("API.Entities.VehicleBrand", b =>
                {
                    b.HasOne("API.Entities.Brand", "Brand")
                        .WithMany("VehicleBrands")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("API.Entities.Vehicle", "Vehicle")
                        .WithOne("VehicleBrand")
                        .HasForeignKey("API.Entities.VehicleBrand", "VehicleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("API.Entities.VehiclePhoto", b =>
                {
                    b.HasOne("API.Entities.Photo", "Photo")
                        .WithOne("VehiclePhoto")
                        .HasForeignKey("API.Entities.VehiclePhoto", "PhotoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Vehicle", "Vehicle")
                        .WithMany("VehiclePhotos")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Photo");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("API.Entities.Brand", b =>
                {
                    b.Navigation("VehicleBrands");
                });

            modelBuilder.Entity("API.Entities.Photo", b =>
                {
                    b.Navigation("VehiclePhoto")
                        .IsRequired();
                });

            modelBuilder.Entity("API.Entities.Vehicle", b =>
                {
                    b.Navigation("VehicleBrand")
                        .IsRequired();

                    b.Navigation("VehiclePhotos");
                });
#pragma warning restore 612, 618
        }
    }
}
