using DOTP_BE.Model;

namespace DOTP_BE.ViewModel.MobileResponses
{
    public class ResponseMessage
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = String.Empty;
    }
    public class NRCMobileResponse :ResponseMessage
    {
        public List<NRC> Data { get; set; }
    }

    public class UserAuthenticateMobileResponse : ResponseMessage
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }
    public class UserMobileResponse  :ResponseMessage
    {
        public User Data { get; set; }
    }
    public class UserOTPResp : ResponseMessage
    {
        public string OtpCode { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
