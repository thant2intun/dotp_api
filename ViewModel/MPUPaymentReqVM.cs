namespace DOTP_BE.ViewModel
{
    public class MPUPaymentReqVM
    {
        public string Version { get; set; }
        public string merchantID { get; set; }
        public string invoiceNo { get; set; }
        public string productDesc { get; set; }
        public string amount { get; set; }
        public int currencyCode { get; set; }
        public string categoryCode { get; set; }
        public string hashValue { get; set; }
        public string cardInfo { get; set; }
        public string FrontendURL { get; set; }
        public string BackendURL { get; set; }
        public string? userDefined1 { get; set; }
        public string? userDefined2 { get; set; }
        public string? userDefined3 { get; set; }
    }
}
