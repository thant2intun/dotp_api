using System.ComponentModel.DataAnnotations;

namespace DOTP_BE.Model
{
    public class JourneyType
    {
        [Key]
        public int JourneyTypeId { get; set; }
        [Required]
        public string JourneyTypeLong { get; set; }
        [Required]
        public string JourneyTypeShort { get; set; }

        public List<Fee> Fees { get; set; }
        public List<LicenseOnly> LicenseOnlys { get; set; }
    }
}
