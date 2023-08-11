namespace DOTP_BE.ViewModel
{
    public class OperatorDetailMobileVM
    {
       public OperatorDetailHead  operatorDetailHead { get; set; }
       public List<CarObject>? carObjects { get; set; }
        public int totalCarCount { get; set; }
        public int totalPage { get; set; }
    }

    public class OperatorDetailHead
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
    }

}
