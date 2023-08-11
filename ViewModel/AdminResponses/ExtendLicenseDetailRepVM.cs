namespace DOTP_BE.ViewModel.AdminResponses
{
    public class ExtendLicenseDetailRepVM
    {
        public string License_NumberLong { get; set; }
        public string LicenseOwner { get; set; }
        public string OfficeLongName { get; set; }
        public int TotalCar { get; set; }
        public int AddTotalCar { get; set; }
        public string NRC_Number { get; set; }
        public string Phone { get; set; }
        //public string? Email { get; set; }
        public string Address { get; set; }
        public string JourneyTypeLong { get; set; }
        public string Township_Name { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string Status { get; set; }
        public string AllowedVehicleWeight { get; set; }


        //License Files
        public string? Temp_AttachFile_NRC { get; set; }
        public string? Temp_AttachFile_M10 { get; set; }
        public string? Temp_AttachFile_OperatorLicense { get; set; }
        public string? Temp_AttachFile_Part1 { get; set; }

        //ChangeLicenseOwner
        public string? LAddress { get; set; }
        public string? Temp_LAddress { get; set; } //temp address
        public string? LTownship_Name { get; set; }
        public string? Temp_LTownship_Name { get; set; } //temp township name

        public string? VehicleNumber { get; set; }

        //ChangeVehicleOwnerAddress
        public string? VehicleOwnerAddress { get; set; }
        public string? Temp_VehicleOwnerAddress { get; set; } //temp

        //public string? Temp_Township_Name { get; set; } //temp
        public string? VehicleLocation { get; set; }
        public string? Temp_VehicleLocation { get; set; }

        //ChangeVehicleOwnerNameChange
        public string? VehicleOwnerName { get; set; }
        public string? Temp_VehicleOwnerName { get; set; }
        public string? VehicleOwnerNRC { get; set; }
        public string? Temp_VehicleOwnerNRC { get; set; }


        //ChangeVehicleType
        public string? VehicleType { get; set; }
        public string? Temp_VehicleType { get; set; }
        public string? VehicleBrand{ get; set; }
        public string? Temp_VehicleBrand { get; set; }
        public string? VehicleWeight { get; set; }
        public string? Temp_VehicleWeight { get; set; }
        public string? Temp_Triangle { get; set; }
        public string? Temp_OwnerBook { get; set; }
        public string? Temp_AttachedFile1 { get; set; }
        public string? Temp_AttachedFile2 { get; set; }
    }
}
