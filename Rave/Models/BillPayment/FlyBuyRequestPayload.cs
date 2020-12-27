namespace RaveDotNet.Models.BillPayment {
    public class FlyBuyRequestPayload: BillPaymentRequestPayload
    {
        public string Country { get; set; }
        public string CustomerId { get; set; }
        public string Reference { get; set; }
        public int Amount { get; set; }
        public int RecurringType { get; set; }
        public bool IsAirtime { get; set; }
        public string BillerName { get; set; }
    }

   
}