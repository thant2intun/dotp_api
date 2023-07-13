using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DOTP_BE.ViewModel
{
    public class CheckoutSessionModel
    {
        public string SessionId { get; set; }
        public string Version { get; set; }
        public string SuccessIndicator { get; set; }
        public string MerchantId { get; set; }
        public string OrderId { get; set; }
        public string SsUrl { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public StringContent data { get; set; }

        public static CheckoutSessionModel toCheckoutSessionModel(string response)
        {
            JObject jObject = JObject.Parse(response);
            CheckoutSessionModel model = jObject["session"].ToObject<CheckoutSessionModel>();
            model.SuccessIndicator = jObject["successIndicator"] != null ? jObject["successIndicator"].ToString() : "";
            return model;

        }
    }
}
