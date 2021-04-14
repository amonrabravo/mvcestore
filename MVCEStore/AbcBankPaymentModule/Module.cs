using MVCEStorePayment;
using System;
using System.Threading.Tasks;

namespace AbcBankPaymentModule
{
    public class Module : PaymentModule
    {
        public override string BankName { get => "AbcBank"; set => base.BankName = value; }

        public override Task<PaymentResult> Payment(PaymentRequest paymentRequest)
        {
            return Task.Run(() =>
            {
                return new PaymentResult
                {
                    Error = null,
                    Succeeded = true
                };
            });
        }
    }
}
