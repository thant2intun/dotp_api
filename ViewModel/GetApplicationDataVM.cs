namespace DOTP_BE.ViewModel
{
    public class GetApplicationDataVM
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string? Status { get; set; }
        public int? LicenseType { get; set; }

        //Pagination
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
