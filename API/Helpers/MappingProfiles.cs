using API.DataTransferObjects.Photo;
using API.DataTransferObjects.Vehicle;
using API.Models;
using AutoMapper;

public class MappingProfiles : Profile {
    public MappingProfiles() {
        CreateMap<Vehicle, VehicleSummaryResponse>()
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))
        ;

        CreateMap<Photo, PhotoSummaryResponse>()
            
        ;
    }   
}