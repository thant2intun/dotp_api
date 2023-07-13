using DOTP_BE.Model;

namespace DOTP_BE.ViewModel
{
    public class VehicleWeightFeeVM
    {
        public int VehicleWeightId { get; set; }
        public string RegistrationTitle { get; set; }
        public int OneToFive { get; set; }
        public int SixToTen { get; set; }
        public int ElevenToTwenty { get; set; }
        public int TwentyOneToThirty { get; set; }
        public int ThirtyOneToFourty { get; set; }
        public int FourtyOneToHundred { get; set; }
        public int HundredOneToFiveHundred { get; set; }
        public int FiveHundredOneToThousand { get; set; }
        public int ThousandOneAndAbove { get; set; }
        public string CreatedBy { get; set; }
    }
}
