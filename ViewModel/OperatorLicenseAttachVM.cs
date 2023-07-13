namespace DOTP_BE.ViewModel
{
    public class OperatorLicenseAttachVM
    {
        public string? Transaction_Id { get; set; } //from frontend
        public string? ChalenNumber { get; set; } //from frontend
        public string licenseNumberLong { get; set; }
        //public string LicenseHolderType { get; set; }
        //public string OperatorName { get; set; }
        //public string LicenseOwner { get; set; }
        public string NRC { get; set; }
        //public string Address { get; set; }
        //public string Township { get; set; }
        //public string? Email { get; set; }
        //public string Phone { get; set; }
        //public string? Fax { get; set; }
        //public string AllowBusinessTitle { get; set; }
        //public int OtherRegistrationOffice_Id { get; set; }
        //public DateTime IssueDate { get; set; }
        //public bool? IsClosed { get; set; }
        public string? FormMode { get; set; }
        //public bool? IsDeleted { get; set; }
        ////public int? VehicleId { get; set; } // for transaction(to get vehicle weight Id) //al cause logic change can get From Vehicl By Transaction iD
        //public int applicant_Id { get; set; }
        //public int selectedExtenYear { get; set; }


        public List<IFormFile> AttachFile_NRC { get; set; }
        public List<IFormFile> AttachFile_M10 { get; set; }
        public List<IFormFile>? AttachFile_RecommandDoc1 { get; set; }
        public List<IFormFile>? AttachFile_RecommandDoc2 { get; set; }
        public List<IFormFile>? AttachFile_OperatorLicense { get; set; }
        public List<IFormFile>? AttachFile_Part1 { get; set; }

        //public int RegistrationOffice_Id { get; set; }
        //public int JourneyType_Id { get; set; }
        //public int PersonInformationId { get; set; }
        //public string? CreatedBy { get; set; }

        //for car attached
        //public List<string> CarAttachedFiles { get; set; }
        public List<CarAttachedFileVM> CarAttachedFiles { get; set; }
    }

    public class CarAttachedFileVM
    {
        public int CreateCarId { get; set; }
        public List<IFormFile>? TriangleFiles { get; set; }
        public List<IFormFile>? OwnerBookFiles { get; set; }
        public List<IFormFile>? AttachedFiles1 { get; set; }
        public List<IFormFile>? AttachedFiles2 { get; set; }

    }
}
