using DOTP_BE.Model;

namespace DOTP_BE.ViewModel
{
    public class PersonInformationVM
    {
        public string Name { get; set; }
        public string NRC_Number { get; set; }
        public string Tsp_Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public DateTime RegisterDate { get; set; }
        public int TownshipId { get; set; }
        public int NRCId { get; set; }
        public string CreatedBy { get; set; }
    }
}
