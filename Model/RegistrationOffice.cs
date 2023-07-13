using System.ComponentModel.DataAnnotations;

namespace DOTP_BE.Model
{
    public class RegistrationOffice
    {
        [Key]
        public int OfficeId { get; set; }
        public string OfficeLongName { get; set; }
        public string OfficeShortName { get; set; }
        public List<LicenseOnly> LicenseOnlys { get; set; }
    }
}
