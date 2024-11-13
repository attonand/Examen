using System.ComponentModel.DataAnnotations;

namespace API.DataTransferObjects.Vehicle
{
    public class VehicleCreateRequest
    {
        [Required(ErrorMessage = "El modelo del vehículo es requerido.")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo del vehículo es requerido.")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "El año del vehículo es requerido.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "El color del vehículo es requerido.")]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "La URL de la foto es requerida.")]
        public List<VehiclePhotoCreateDto> Photos { get; set; } = [];

    }

    public class VehiclePhotoCreateDto {
        public string Url { get; set; } = string.Empty;
    }

}
