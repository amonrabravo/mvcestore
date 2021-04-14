using System;
using System.Threading.Tasks;

namespace MVCEStorePayment
{
    public interface IPaymentModule
    {
        string BankName { get; set; }

        Task<PaymentResult> Payment(PaymentRequest paymentRequest);

    }
}
