using MVCEStorePayment;
using System;
using System.Threading.Tasks;

namespace XyzBankPaymentModule
{
    public class Module : PaymentModule
    {
        public override string BankName { get => "XyzBank"; set => base.BankName = value; }

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
