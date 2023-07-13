namespace DOTP_BE.ViewModel
{
    public class UserVM
    {
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
        public bool RegisterWithNrc { get; set; }
        public string NRC_Number { get; set; }
        public int? NRCId { get; set; }  //person and organization(no nrcid)

        public int? PersonInformationId { get; set; }
        public bool? IsEmail { get; set; } //to know that use is confirm with phone or email
    }
}
