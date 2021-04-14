using MVCEStorePayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCEStoreWeb.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> Payment(PaymentRequest paymentRequest);
    }
}
