using DOTP_BE.Model;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DOTP_BE.Models
{
    public class OperatorDetail : BaseModel
    {
        [Key]
        public int OperatorId { get; set; }
        public string Transaction_Id { get; set; } //int before(7/2/2023)
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
        public string? DesiredRoute { get; set; }
        public string? Notes { get; set; }
        //public string ApplyLicenseType { get; set; } //from LicenseType
        public int ApplyLicenseType { get; set; }
        public bool? IsClosed { get; set; }
        public bool? IsDeleted { get; set; }
        public string? FormMode { get; set; }

        public int PersonInformationId { get; set; }
        public PersonInformation PersonInformation { get; set; }
        public int? VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
