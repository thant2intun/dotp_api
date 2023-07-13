using DOTP_BE.Model;

namespace DOTP_BE.ViewModel
{
    public class CreateCarVM
    {
        public int createCarId { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleType { get; set; }
        public string VehicleWeight { get; set; }
        public string VehicleLocation { get; set; }
        public string VehicleOwnerName { get; set; }
        public string VehicleOwnerNRC { get; set; }
        public string VehicleOwnerAddress { get; set; }

        public string Township { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public int PersonInformationId { get; set; }
    }

    public class UpdateCreateCar
    {
        public int createCarId { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleType { get; set; }
        public string VehicleWeight { get; set; }
    }
}
