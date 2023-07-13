using System.ComponentModel.DataAnnotations;

namespace DOTP_BE.Model
{
    public class NRC
    {
        [Key]
        public int NRCId { get; set; }
        public string NRCCode { get; set; }
        public string NRCEnglishCode { get; set; }
        public string NRCMyanmarCode { get; set; }
        public int NRCNumber { get; set; }

        public List<PersonInformation> PersonInformations { get; set; }
    }
}
