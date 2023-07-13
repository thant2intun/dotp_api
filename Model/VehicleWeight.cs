using System.ComponentModel.DataAnnotations;

namespace DOTP_BE.Model
{
    public class VehicleWeight : BaseModel
    {
        [Key]
        public int VehicleWeightId { get; set; } // ပျက်ဆီး + ပျောက်ဆုံး ၂ ခုလုံးကို ID 5 သာသုံးရန်
        public string VehicleType { get; set; }

    }
}
