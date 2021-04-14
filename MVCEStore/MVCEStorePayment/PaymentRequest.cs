namespace MVCEStorePayment
{
    public class PaymentRequest
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public int ExpireMonth { get; set; }
        public int ExpireYear { get; set; }
        public string SecurityCode { get; set; }
        public int Instalments { get; set; }
        public decimal Amount { get; set; }
    }
}
