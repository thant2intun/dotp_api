using System.ComponentModel.DataAnnotations;

namespace DOTP_BE.Model
{
    public class Delivery
    {
        [Key]
        public int DeliveryId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Township_Name { get; set; }

        public List<LicenseOnly> LicenseOnlys { get; set; }
    }
}
