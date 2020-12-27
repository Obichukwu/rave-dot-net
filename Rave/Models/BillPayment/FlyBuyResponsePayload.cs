namespace RaveDotNet.Models.BillPayment {
    public class FlyBuyResponsePayload
    {
        public string MobileNumber { get; set; }
        public int Amount { get; set; }
        public string Network { get; set; }
        public string TransactionReference { get; set; }
        public string PaymentReference { get; set; }
        public object BatchReference { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public object Reference { get; set; }
    }
}