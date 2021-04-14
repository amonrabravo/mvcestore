using System;
using System.Threading.Tasks;

namespace MVCEStorePayment
{
    public interface IPaymentModule
    {
        string BankName { get; set; }

        public string MerchantId { get; set; }
        public string Password { get; set; }

        Task<PaymentResult> Payment(PaymentRequest paymentRequest);

    }
}
