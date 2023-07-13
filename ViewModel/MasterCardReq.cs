using Microsoft.VisualBasic;

namespace DOTP_BE.ViewModel
{
    public class MasterCardReq
    {
        public string apiOperation { get; set; } = "INITIATE_CHECKOUT";
        //public string apiPassword { get; set; } //
        //public string apiUsername { get; set; } //merchant.<your_merchant_id>
        //public string merchant { get; set; } //<your_merchant_id>
        public Order order { get; set; }

        public Interaction interaction { get; set; }

    }

    public class Order
    {
        public string id { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        //public string description { get; set; }

    }

    public class Interaction
    {

        public string operation { get; set; }

        //public Merchant merchant { get; set; }
    }

    public class Merchant
    {

        public string name  { get; set; }
    }
}
