namespace DOTP_BE.ViewModel.AdminResponses
{
    public class ExtendLicenseDetailRepVM
    {
        public string License_Number { get; set; } //From LicenseOnly
        public string LicenseOwner { get; set; }
        public string OfficeLongName { get; set; }
        public int TotalCar { get; set; }
        public int AddTotalCar { get; set; }
        public string NRC_Number { get; set; } //From LicenseOnly
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; }
        public string JourneyTypeLong { get; set; }
        public string Township_Name { get; set; }
        public DateTime? ExpiryDate { get; set; }

        //License Files
        public string AttachFile_NRC { get; set; }
        public string AttachFile_M10 { get; set; }
        public string AttachFile_OperatorLicense { get; set; }
        public string AttachFile_Part1 { get; set; }

        //ChangeLicenseOwner
        public string l_O_Address { get; set; }
        public string l_O_Township_Name { get; set; }
        public string l_N_Address { get; set; }
        public string l_N_Township_Name { get; set; }

        //ChangeVehicleOwnerAddress
        public string O_VehicleOwnerAddress { get; set; }
        public string O_VehicleLocation { get; set; }
        public string N_VehicleOwnerAddress { get; set; }
        public string N_VehicleLocation { get; set; }

        //ChangeVehicleOwnerName
        public string O_VehicleOwnerName { get; set; }
        public string O_VehicleOwnerNRC { get; set; }
        //public string O_VehicleOwnerAddress { get; set; }
        public string N_VehicleOwnerName { get; set; }
        public string N_VehicleOwnerNRC { get; set; }
        //public string N_VehicleOwnerAddress { get; set; }


        //ChangeVehicleType

    }
}
