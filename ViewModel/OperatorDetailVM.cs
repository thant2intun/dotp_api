namespace DOTP_BE.ViewModel
{
    public class OperatorDetailVM
    {
        public string Transaction_Id { get; set; } //string before 07/03/2023
        public string LicenseHolderType { get; set; }
        public string OperatorName { get; set; }
        public string? AllowBusinessTitle { get; set; }
        public string Address { get; set; }
        public DateTime ApplyDate { get; set; }
        public string LicenseOwner { get; set; }
        public int RegistrationOffice_Id { get; set; }
        public string NRC { get; set; }
        public int applicant_Id { get; set; }
        public string Township { get; set; }
        public string Phone { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public int JourneyType_Id { get; set; }
        public int TotalCar { get; set; }
        public string DesiredRoute { get; set; }
        public string Notes { get; set; }
        public int ApplyLicenseType { get; set; }
        public bool? IsClosed { get; set; }
        public bool? IsDeleted { get; set; }
        public string? FormMode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public int? VehicleId { get; set; }
        public int PersonInformationId { get; set; }

        //For Response FE
        public string? LicenseNumberLong { get; set; }
        public string? OfficeName { get; set; }
        public string? JourneyTypeName { get; set; }
        //public int? JourneyTypeId { get; set; }
        public string ChalenNumber { get; set; }

        // For response Vehicle Attachement file
        public List<CarObject>? CarObjects { get; set; }



        // For Car Object Details
        //public List<CarObjectDetails> CarObjectDetails { get; set; }
    }

    //public class ExtenseCarFile
    //{
    //    public string ? AttachFile_NRC;
    //    public string? AttachFile_M10;
    //    public string? AttachFile_RecommandDoc1;
    //    public string? AttachFile_RecommandDoc2;
    //    public string? AttachFile_OperatorLicense;
    //    public string? AttachFile_Part1;
    //}

    //public class CarObject
    //{
    //    public int CreateCarId { get; set; }
    //    public string VehicleNumber { get; set; }
    //    public string? VehicleBrand { get; set; }
    //    public string? VehicleType { get; set; }
    //    public string? VehicleOwnerName { get; set; }
    //    public DateTime? ExpiredDate { get; set; }
    //    public string AllowedWeight { get; set; }

    //}

    public class CarObject
    {
        public int Id { get; set; }//add by ema
        public int CreateCarId { get; set; }
        public string VehicleNumber { get; set; }
        public string? VehicleBrand { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleOwnerName { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string? AllowedWeight { get; set; }

        public string? vehicleOwnerAddress { get; set; }
        public string? vehicleAddress { get; set; }
        public string? vehicleWeight { get; set; } 
        public string? vehicleOwnerNRC { get; set; }
        public string? OwnerBook { get; set; } // for oll data view
        public string? Triangle { get; set; }  // for oll data view
        public string? AttachedFile1 { get; set; } // for oll data view
        public string? AttachedFile2 { get; set; } // for oll data view



    }

    //public class CarObjectDetails
    //{
    //    public int CreateCarId { get; set; }
    //    public string VehicleNumber { get; set; }
    //    public string? VehicleBrand { get; set; }
    //    public string? VehicleType { get; set; }
    //    public string? VehicleOwnerName { get; set; }
    //    public DateTime? ExpiredDate { get; set; }
    //    public string AllowedWeight { get; set; }

    //    public string? vehicleOwnerAddress { get; set; }
    //    public string? vehicleAddress { get; set; }
    //    public string? vehicleWeight { get; set; }
    //    public string? vehicleOwnerNRC { get; set; }


    //    public string? OwnerBook { get; set; }
    //    public string? Triangle { get; set; }
    //    public string? AttachedFile2 { get; set; }

    //}

}
