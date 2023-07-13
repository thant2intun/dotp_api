namespace DOTP_BE.ViewModel
{
    public class ExtenseCarVMList
    {
        public string? Transaction_Id { get; set; } //from frontend
        public string? ChalenNumber { get; set; } //from frontend

        public List<IFormFile> AttachFile_NRC { get; set; }
        public List<IFormFile> AttachFile_M10 { get; set; }
        public List<IFormFile>? AttachFile_RecommandDoc1 { get; set; }
        public List<IFormFile>? AttachFile_RecommandDoc2 { get; set; }
        public List<IFormFile> AttachFile_OperatorLicense { get; set; }
        public List<IFormFile> AttachFile_Part1 { get; set; }

        public string LicenseNumberLong { get; set; }
        public string License_Number { get; set; }
        public string LicenseOwner { get; set; }
        public string NRC_Number { get; set; }
        public string Address { get; set; }
        public string Township_Name { get; set; }
        public string Phone { get; set; }
        public string? Fax { get; set; }
        public string AllowBusinessTitle { get; set; }
        public DateTime IssueDate { get; set; }
        //public bool? isClosed { get; set; }
        //public bool isDeleted { get; set; }
        public string CreatedBy { get; set; }
        public int RegistrationOfficeId { get; set; }
        public int JourneyTypeId { get; set; }
        public int PersonInformationId { get; set; }


        public List<ExtenseCarVM> ExtenseCarVMs { get; set; } 
    }
    public class ExtenseCarVM
    {
        public int? createCarId { get; set; }
        public DateTime expiredDate { get; set; }
        public string vehicleBrand { get; set; }
        public string vehicleNumber { get; set; }
        public string vehicleOwnerName { get; set; }
        public string? vehicleType { get; set; }
        public string allowedWeight { get; set; }
        public string? ownerAddress { get; set; }
        public string? vehicleAddress { get; set; }
        public string? vehicleWeight { get; set; }
        public string vehicleOwnerNRC { get; set; }

        public List<IFormFile>? AttachedFile1 { get; set; }
        public List<IFormFile>? AttachedFile2 { get; set; }

    }
}


