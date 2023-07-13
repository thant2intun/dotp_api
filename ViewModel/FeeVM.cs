using DOTP_BE.Model;

namespace DOTP_BE.ViewModel
{
    public class FeeVM
    {
        public int RegistrationFees { get; set; }
        public int RegistrationCharges { get; set; }
        public int CertificateFees { get; set; }
        public int PartOneFees { get; set; }
        public int PartTwoFees { get; set; }
        public int TriangleFees { get; set; }
        public int MinCars { get; set; }
        public int MaxCars { get; set; }
        public int VehicleWeightId { get; set; }
        public int JourneyTypeId { get; set; }
    }
}
