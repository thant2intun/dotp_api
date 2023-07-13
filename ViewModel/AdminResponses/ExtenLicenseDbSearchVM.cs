namespace DOTP_BE.ViewModel.AdminResponses
{
    public class ExtenLicenseDbSearchVM
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
        public string? FormMode { get; set; }
        public int? JourneyType { get; set; }
        public int? LicenseType { get; set; }
    }
}
