using System.ComponentModel.DataAnnotations;

namespace DOTP_BE.Model
{
    public class Township
    {
        [Key]
        public int TownshipId { get; set; }
        [Required]
        public string TownshipCode { get; set; }
        [Required]
        public string TownshipNameEnglish { get; set; }
        [Required]
        public string TownshipNameMyanmar { get; set; }
        [Required]
        public string Region { get; set; }

        public List<PersonInformation> PersonInformations { get; set; }
        //public List<LicenseOnly> LicenseOnlys { get; set; }
    }
}
