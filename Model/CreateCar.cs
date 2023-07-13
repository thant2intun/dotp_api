 using System.ComponentModel.DataAnnotations;
namespace DOTP_BE.Model
{
    public class CreateCar : BaseModel
    {
        [Key]
        public int CreateCarId { get; set; }
        public string VehicleNumber { get; set; }
        public string? VehicleBrand { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleWeight { get; set; }
        public string? VehicleLocation { get; set; }
        public string? VehicleOwnerName { get; set; }
        public string? VehicleOwnerNRC { get; set; }
        public string? VehicleOwnerAddress { get; set; }
        public bool? IsDeleted { get; set; }
        public int? PersonInformationId { get; set; }
        public PersonInformation PersonInformation { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}