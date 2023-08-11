using System.ComponentModel.DataAnnotations;
namespace DOTP_BE.Model
{
    public class LicenseOnly : BaseModel
    {
        [Key]
        public int LicenseOnlyId { get; set; }
        public string Transaction_Id { get; set; }
        public string License_Number { get; set; }
        public string LicenseOwner { get; set; }
        public string NRC_Number { get; set; }
        public string Address { get; set; }
        public string? Temp_Address { get; set; } //temp address
        public string Township_Name { get; set; }
        public string? Temp_Township_Name { get; set; } //temp township name
        public string Phone { get; set; }
        public string? Fax { get; set; }
        public string? AllowBusinessTitle { get; set; }
        public int OtherRegistrationOffice_Id { get; set; }
        public DateTime IssueDate { get; set; }
        public bool? IsClosed { get; set; }
        public string? FormMode { get; set; }
        public bool? IsDeleted { get; set; }
        public string AttachFile_NRC { get; set; }
        public string? Temp_AttachFile_NRC { get; set; } //temp
        public string AttachFile_M10 { get; set; }
        public string? Temp_AttachFile_M10 { get; set; } //temp
        public string AttachFile_OperatorLicense { get; set; }
        public string? Temp_AttachFile_OperatorLicense { get; set; } //temp
        public string AttachFile_Part1 { get; set; }
        public string? Temp_AttachFile_Part1 { get; set; } //temp
        public string AttachFile_RecommandDoc1 { get; set; }
        public string? Temp_AttachFile_RecommandDoc1 { get; set; }
        public string AttachFile_RecommandDoc2 { get; set; }
        public string Temp_AttachFile_RecommandDoc2 { get; set; }
        public string AttachFile_RecommandDoc3 { get; set; }
        public string AttachFile_RecommandDoc4 { get; set; }
        public string AttachFile_RecommandDoc5 { get; set; }

        public int RegistrationOfficeId { get; set; }
        public RegistrationOffice RegistrationOffice { get; set; }
        public int JourneyTypeId { get; set; }
        public JourneyType JourneyType { get; set; }
        public int? DeliveryId { get; set; }
        public Delivery Delivery { get; set; }
        public int PersonInformationId { get; set; }
        public PersonInformation PersonInformation { get; set; }

    }
}
