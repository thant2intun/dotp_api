namespace DOTP_BE.ViewModel
{
    public class CBPayQrRequest
    {
        public string reqId { get; set; } //Length = 32
        public string merId { get; set; }//Length = 16
        public string subMerId { get; set; }//Length = 16
        public string terminalId { get; set; }//Length = 8
        public string transAmount { get; set; }//Length = 13
        public string transCurrency { get; set; }//Length = 3
        public string ref1 { get; set; }//Length = 25
        public string ref2 { get; set; }//Length = 25
    }
}
