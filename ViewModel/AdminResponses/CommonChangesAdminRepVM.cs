namespace DOTP_BE.ViewModel.AdminResponses
{
    public class CommonChangesAdminRepVM
    {
        public string Transaction_Id { get; set; } //string before 07/02/2023
        public string LicenseOwner { get; set; }
        public string OfficeLongName { get; set; }
        public int TotalCar { get; set; }
        public int AddedTotalCar { get; set; }
        public string NRC_Number { get; set; } //From LicenseOnly
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; }
        public string JourneyTypeLong { get; set; }
        public string Township_Name { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string AttachFile_NRC { get; set; }
        public string AttachFile_M10 { get; set; }
        public string AttachFile_OperatorLicense { get; set; }
        public string AttachFile_Part1 { get; set; }
        public A_ChangeLOwnerAddress_Sub1? ChangeLicenseAddress { get; set; }
        public List<A_ChangeVOwnerAddress_Sub2>? ChangeVehicleAddress { get; set; }
        public A_ChangeVehicleType_Sub3? ChangeVehicleType { get; set; }

    }

    public class A_ChangeLOwnerAddress_Sub1
    {
        public string? L_Address { get; set; }
        public string? L_Township_Name { get; set; }
        public string Address { get; set; }
        public string Township_Name { get; set; } //need to send from FE
    }

    public class A_ChangeVOwnerAddress_Sub2
    {
        public int CreateCarId { get; set; }
        public string Old_VehicleOwnerAddress { get; set; }
        public string VehicleOwnerAddress { get; set; }

        public string Old_VehicleLocation { get; set; }
        public string VehicleLocation { get; set; }
    }

    public class A_ChangeVehicleType_Sub3
    {
        public string LicenseNumberLong { get; set; }
        public string VehicleNumber { get; set; }
        public string? VehicleOwnerName { get; set; }
        public string? AttachedFile2 { get; set; }
        public string? Triangle { get; set; }
        public string? OwnerBook { get; set; }
        public string AttachedFile1 { get; set; }
        public int CreateCarId { get; set; }
        public string Old_VehicleType { get; set; }
        public string VehicleType { get; set; }
        public string Old_VehicleBrand { get; set; }
        public string VehicleBrand { get; set; } // for oll data view
        public string Old_VehicleWeight { get; set; }  // for oll data view
        public string? VehicleWeight { get; set; } // for oll data view
    }
}
