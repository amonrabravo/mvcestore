using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCEStorePayment
{
    public abstract class PaymentModule : IPaymentModule
    {
        public virtual string BankName { get; set; } = "";

        public abstract Task<PaymentResult> Payment(PaymentRequest paymentRequest);

        public string MerchantId { get; set; }
        public string Password { get; set; }

    }
}
