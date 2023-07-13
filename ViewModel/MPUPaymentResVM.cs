namespace DOTP_BE.ViewModel
{

    //mwl For MPGS Result Response  
    public class MPUPaymentResVM
    {
        public string merchantID { get; set; } 
        public string respCode { get; set; } 
        public string pan { get; set; } 
        public string amount { get; set; } 
        public string invoiceNo { get; set; } 
        public string tranRef { get; set; } 
        public string approvalCode { get; set; } 
        public string dateTime { get; set; }
        public string status { get; set; } 
        public string failReason { get; set; } 
        public string userDefined1 { get; set; }
        public string userDefined2 { get; set; } 
        public string userDefined3 { get; set; } 
        public string hashValue { get; set; } 
    }

    public class MCR_Address
    {
        public string city { get; set; }
        public string country { get; set; }
        public string postcodeZip { get; set; }
        public string stateProvince { get; set; }
        public string street { get; set; }
    }

    public class MCR_Billing
    {
        public MCR_Address address { get; set; }
    }

    public class MCR_Customer
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class MCR_Device
    {
        public string browser { get; set; }
        public string ipAddress { get; set; }
    }

    public class MCR_Expiry
    {
        public string month { get; set; }
        public string year { get; set; }
    }

    public class MCR_Card
    {
        public string brand { get; set; }
        public MCR_Expiry expiry { get; set; }
        public string fundingMethod { get; set; }
        public string nameOnCard { get; set; }
        public string number { get; set; }
        public string scheme { get; set; }
    }

    public class MCR_Provided
    {
        public MCR_Card card { get; set; }
    }

    public class MCR_SourceOfFunds
    {
        public MCR_Provided provided { get; set; }
        public string type { get; set; }
    }

    public class MCR__3DSecure
    {
        public string acsEci { get; set; }
        public string authenticationStatus { get; set; }
        public string authenticationToken { get; set; }
        public string enrollmentStatus { get; set; }
        public string xid { get; set; }
    }

    public class MCR_AuthorizationResponse
    {
        public string cardSecurityCodeError { get; set; }
        public string date { get; set; }
        public string posData { get; set; }
        public string posEntryMode { get; set; }
        public string processingCode { get; set; }
        public string responseCode { get; set; }
        public string stan { get; set; }
        public string time { get; set; }
    }

    public class MCR_Order
    {
        public int amount { get; set; }
        public DateTime creationTime { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string id { get; set; }
        public string status { get; set; }
        public int totalAuthorizedAmount { get; set; }
        public int totalCapturedAmount { get; set; }
        public int totalRefundedAmount { get; set; }
    }

    public class MCR_CardSecurityCode
    {
        public string acquirerCode { get; set; }
        public string gatewayCode { get; set; }
    }

    public class MCR_Response
    {
        public string acquirerCode { get; set; }
        public string acquirerMessage { get; set; }
        public MCR_CardSecurityCode cardSecurityCode { get; set; }
        public string gatewayCode { get; set; }
    }

    public class MCR_Acquirer
    {
        public int batch { get; set; }
        public string date { get; set; }
        public string id { get; set; }
        public string merchantId { get; set; }
        public string settlementDate { get; set; }
        public string timeZone { get; set; }
    }

    public class MCR_Transaction
    {
        public MCR_Acquirer acquirer { get; set; }
        public int amount { get; set; }
        public string authorizationCode { get; set; }
        public string currency { get; set; }
        public string frequency { get; set; }
        public string id { get; set; }
        public string receipt { get; set; }
        public string source { get; set; }
        public string terminal { get; set; }
        public string type { get; set; }
        public MCR__3DSecure _3DSecure { get; set; }
        public string _3DSecureId { get; set; }
        public MCR_AuthorizationResponse authorizationResponse { get; set; }
        public MCR_Billing billing { get; set; }
        public MCR_Customer customer { get; set; }
        public MCR_Device device { get; set; }
        public string gatewayEntryPoint { get; set; }
        public string merchant { get; set; }
        public MCR_Order order { get; set; }
        public MCR_Response response { get; set; }
        public string result { get; set; }
        public MCR_SourceOfFunds sourceOfFunds { get; set; }
        public DateTime timeOfRecord { get; set; }
        public MCR_Transaction transaction { get; set; }
        public string version { get; set; }
    }

    public class MasterCardCheckTransactionResponse
    {
        public int amount { get; set; }
        public MCR_Billing billing { get; set; }
        public DateTime creationTime { get; set; }
        public string currency { get; set; }
        public MCR_Customer customer { get; set; }
        public string description { get; set; }
        public MCR_Device device { get; set; }
        public string id { get; set; }
        public string merchant { get; set; }
        public string result { get; set; }
        public MCR_SourceOfFunds sourceOfFunds { get; set; }
        public string status { get; set; }
        public int totalAuthorizedAmount { get; set; }
        public int totalCapturedAmount { get; set; }
        public int totalRefundedAmount { get; set; }
        public List<MCR_Transaction> transaction { get; set; }
    }
}
