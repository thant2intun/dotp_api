using System.ComponentModel.DataAnnotations;

namespace DOTP_BE.Model
{
    public class Representative
    {
        [Key]
        public int RepresentativeId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string NRC_Number { get; set; }
        public int? NRCId { get; set; }
        public NRC NRC { get; set; }

        public List<Vehicle> Vehicles { get; set; }
    }
}
