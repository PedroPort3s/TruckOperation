using System.ComponentModel.DataAnnotations;

namespace BlazorClient.ViewModel
{
    public class TruckViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Range(1928, 2999, ErrorMessage = "Year of Manufacture must be between 1928 and 2999.")]
        public int YearOfManufacture { get; set; }

        [Required]
        [StringLength(17, ErrorMessage = "Chassis Code cannot exceed 17 characters.")]
        public string ChassisCode { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Color cannot exceed 20 characters.")]
        public string Color { get; set; }

        [Required]
        [Range(1,999, ErrorMessage = "A model is required")]
        public int ModelId { get; set; }

        [Range(1, 999, ErrorMessage = "A plant is required")]
        public int PlantId { get; set; }
    }
}
