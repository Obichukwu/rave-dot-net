using System.Collections.Generic;

namespace RaveDotNet.Models.BillPayment {
    public class FlyBuyBulkRequestPayload : BillPaymentRequestPayload
    {
        public string BatchReference { get; set; }
        public string CallBackUrl { get; set; }
        public List<FlyBuyRequestPayload> Requests { get; set; }
    }
}