namespace MVCEStorePayment
{
    public class PaymentResult
    {
        public bool Succeeded { get; set; } = true;
        public string Error { get; set; }
    }
}