using API.DTOs;
using API.DTOs.Brand;
using API.DTOs.Photo;
using API.DTOs.Vehicle;
using API.Entities;
using AutoMapper;

public class MappingProfiles : Profile {
    public MappingProfiles() {
        CreateMap<Vehicle, VehicleDto>()
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.VehiclePhotos.Select(x => x.Photo)))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.VehicleBrand))
        ;

        CreateMap<Photo, PhotoDto>()
            
        ;

        CreateMap<VehicleBrand, BrandDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Brand.Id))
        ;

        CreateMap<VehicleBrand, OptionDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Brand.Id))
        ;

        CreateMap<Brand, BrandDto>()

        ;

        CreateMap<Brand, OptionDto>()

        ;
    }   
}