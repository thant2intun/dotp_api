using DOTP_BE.Common;

namespace DOTP_BE.ViewModel
{
    public class CommonChangesVM
    {
        public LicenseAttachedFilesVM LicenseAttachedFiles { get; set; }
        public string LicenseNumberLong { get; set; }
        public string NRC_Number { get; set; }
        public string FormMode { get; set; }
        public bool? TakeNewRecord { get; set; }
        public int UserId { get; set; } //for summary page

        //for vehicle owner address change
        public List<ChangeVOwnerAddress_Sub1>? ChangeVehicleAddress { get; set; }

        //for license owner address change
        public ChangeLOwnerAddress_Sub2? ChangeLicenseAddress { get; set; }

        //for vehicle type change
        public List<ChangeVehicleType_Sub3>? ChangeVehicleType { get; set; }

        //for vehicle owner name change
        public List<ChangeVehicleName_Sub4>? ChangeVehicleOwnerName { get; set; }

        //for Add new car
        public List<AddNewCar_Sub5>? AddNewCars { get; set; }

        //For decrease car 
        public List<DecreaseCar_Sub6>? DecreaseCars { get; set; }
        public List<DecreaseCarOver2ton_Sub7>? DecreaseCarsOver2ton { get; set; }

        //public DecreaseCarOver2ton_Sub7? DecreaseCarsOver2ton { get; set; }

        //for Extend Operator License
        public List<ExtendOperatorLicense_Sub8>? ExtendOperatorLicense { get; set; }

        //for Extend Vehicle License
        public List<ExtendVehicleLicense_Sub8>? ExtendVehicleLicense { get; set; }
    } 

    public class ChangeVOwnerAddress_Sub1
    {
        public int CreateCarId { get; set; }
        public string VehicleOwnerAddress { get; set; }
        public string Township_Name { get; set; } //need to send from FE
    }

    public class ChangeLOwnerAddress_Sub2
    {
        public List<int> vehicleIdList { get; set; }
        public string Address { get; set; }
        public string Township_Name { get; set; }
    }

    public class ChangeVehicleType_Sub3
    {
        public int VehicleId { get; set; }
        public string? VehicleType { get; set; }
        public string?  VehicleBrand { get; set; }
        public string? VehicleWeight { get; set; }
        //public string? OwnerBook { get; set; } // for oll data view
        //public string? Triangle { get; set; }  // for oll data view
        //public string? AttachedFile1 { get; set; } // for oll data view
        //public string? AttachedFile2 { get; set; } // for oll data view
        public List<IFormFile>? NewTriangleFiles { get; set; }
        public List<IFormFile>? NewOwnerBookFiles { get; set; }
        public List<IFormFile>? NewAttachedFiles1 { get; set; }
        public List<IFormFile>? NewAttachedFiles2 { get; set; }
    }

    public class ChangeVehicleName_Sub4
    {
        public int VehicleId { get; set; }
        public string VehicleOwnerAddress { get; set; }
        public string VehicleOwnerName { get; set; }
        public string VehicleOwnerNRC { get; set; }
    }

    public class AddNewCar_Sub5
    {
        public string vehicleBrand { get; set; }
        public string vehicleNumber { get; set; }
        public string vehicleOwnerName { get; set; }
        public string vehicleType { get; set; }
        public string vehicleOwnerAddress { get; set; }
        public string vehicleLocation { get; set; }
        public string vehicleWeight { get; set; }
        public string vehicleOwnerNRC { get; set; }
        public string expireDate { get; set; }
        public List<IFormFile>? OwnerBookFiles { get; set; }
        public string? OldOwnerBookFiles { get; set; }
        public List<IFormFile>? AttachedFiles1 { get; set; }
        public string? OldAttachedFiles1 { get; set; }
    }

    public class DecreaseCar_Sub6
    {
        public int VehicleID { get; set; }
        public List<IFormFile>? NewOwnerBook { get; set; }
        public List<IFormFile>? NewTriangle { get; set; }
        public List<IFormFile>? NewAttachedFile2 { get; set; }
    }

    public class DecreaseCarOver2ton_Sub7
    {
        //public List<int> VehicleIdList { get; set; }
        //public int RecieverVehicleId { get; set; }
        public int VehicleID { get; set; }
        public List<IFormFile>? NewOwnerBook { get; set; }
        public List<IFormFile>? NewTriangle { get; set; }
        public List<IFormFile>? NewAttachedFile2 { get; set; }
    }

    public class ExtendOperatorLicense_Sub8
    {
        public int VehicleId { get; set; }
        public List<IFormFile>? TriangleFiles { get; set; }
        public List<IFormFile>? OwnerBookFiles { get; set; }
        public List<IFormFile>? AttachedFiles1 { get; set; }
        public List<IFormFile>? AttachedFiles2 { get; set; }
    }

    public class ExtendVehicleLicense_Sub8
    {
        public int VehicleId { get; set; }
        public List<IFormFile> TriangleFiles { get; set; }
        public List<IFormFile> OwnerBookFiles { get; set; }
        public List<IFormFile> AttachedFiles1 { get; set; }
        public List<IFormFile> AttachedFiles2 { get; set; }
    }
}
