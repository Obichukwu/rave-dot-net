using System;

namespace RaveDotNet.Models.Payment
{
    public class PaymentRequestModel
    {
        public decimal Amount { get; set; }
        public string CustomDescription { get; set; }
        public string CustomLogo { get; set; }
        public string CustomTitle { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerFirstname { get; set; }
        public string CustomerLastname { get; set; }
        public string CustomerPhone { get; set; }
        public string PaymentMethod { get; set; } = "both";
        public string PayButtonText { get; set; }

        public Func<string> GetTransactionReference { get; set; }

        private string _transactionReference;

        public string TransactionReference { 
            get {
                if (this.GetTransactionReference != null) {
                    return this.GetTransactionReference();
                }
                else {
                    if (String.IsNullOrEmpty(_transactionReference)) {
                        _transactionReference = Guid.NewGuid().ToString();
                        return _transactionReference;
                    }
                    else return _transactionReference;
                }
            }
        }
    }
}
