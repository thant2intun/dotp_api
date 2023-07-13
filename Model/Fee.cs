using System.ComponentModel.DataAnnotations;
namespace DOTP_BE.Model
{
    public class Fee 
    {
        [Key]
        public int FeesId { get; set; }
        public int RegistrationFees { get; set; }
        public int RegistrationCharges { get; set; }
        public int CertificateFees { get; set; }
        public int PartOneFees { get; set; }
        public int PartTwoFees { get; set; }
        public int TriangleFees { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int MinCars { get; set; }
        public int MaxCars { get; set; }

        public int JourneyTypeId { get; set; }
        public JourneyType JourneyType { get; set; }
        public int VehicleWeightId { get; set; }
        public VehicleWeight VehicleWeight { get; set; }
    }
}
