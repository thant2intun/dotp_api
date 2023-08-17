using System.ComponentModel.DataAnnotations;

namespace DOTP_BE.Model
{
    public class Summary
    {
        [Key]
        public int SId { get; set; }
        public string LicenseNumberLong { get; set; }
        public string FormMode { get; set; }
        public int TotalCar { get; set; }
        public string TransactionId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
