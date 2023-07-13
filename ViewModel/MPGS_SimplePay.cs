namespace DOTP_BE.ViewModel
{
    public class MPGS_SimplePay
    {
        //cardNumber : string ,
        //expMonth : number ,
        //expYear : number,
        //securityCode :string ,
        //amount : number,
        //chalenNumber :string

        public string cardNumber { get; set; }
        public string expMonth { get; set; }
        public string expYear { get; set; }
        public string securityCode { get; set; }
        public decimal amount { get; set; }
        public string chalenNumber { get; set; }
    }
}
