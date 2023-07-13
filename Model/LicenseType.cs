using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DOTP_BE.Model
{
    public class LicenseType
    {

        [Key]
        public int LicenseTypeId { get; set; }

        [Required]
        public string LicenseTypeLong { get; set; }

        [Required]
        public string LicenseTypeShort { get; set; }
        public List<Vehicle> Vehicles { get; set; }
    }
}
