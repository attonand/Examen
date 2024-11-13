using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Vehicle
{
    public class VehicleCreateDto
    {
        [Required(ErrorMessage = "La marca del vehículo es requerido.")]
        public OptionDto? Brand { get; set; } = null;

        [Required(ErrorMessage = "El modelo del vehículo es requerido.")]
        public string? Model { get; set; } = null;

        [Required(ErrorMessage = "El año del vehículo es requerido.")]
        public int? Year { get; set; } = null;

        [Required(ErrorMessage = "El color del vehículo es requerido.")]
        public string? Color { get; set; } = null;

        [Required(ErrorMessage = "La URL de la foto es requerida.")]
        public List<VehiclePhotoCreateDto> Photos { get; set; } = [];
    }

    public class VehiclePhotoCreateDto {
        public string? Url { get; set; } = null;
    }
}
