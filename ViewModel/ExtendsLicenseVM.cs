namespace DOTP_BE.ViewModel
{
    public class ExtendsLicenseVM
    {
        //public int No { get; set; }
        public int OperatorId { get; set; } //OperatorDetail's
        public string? LicenseNumberLong { get; set; } //Vehicle's
        public string? RegistrationOfficeName { get; set; } //Vehicle's/LicenseOnly
        public DateTime? ExpiryDate { get; set; } //Vehicle's
        public int TotalCar { get; set; } //OperatorDetail's
    }
}
