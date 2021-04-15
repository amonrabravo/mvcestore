using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MVCEStorePayment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MVCEStoreWeb.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IConfiguration configuration;
        private readonly IDictionary<string, IPaymentModule> modules = new Dictionary<string, IPaymentModule>();

        public PaymentService(
            IWebHostEnvironment hostEnvironment,
            IConfiguration configuration
            )
        {
            this.hostEnvironment = hostEnvironment;
            this.configuration = configuration;
            var files = Directory.GetFiles(Path.Combine(hostEnvironment.WebRootPath, "paymentmodules", "net5.0"), "*.dll").ToList();
            files
                .ForEach(p =>
                {
                    Assembly
                    .LoadFile(p)
                    .GetTypes()
                    .Where(q => !q.IsAbstract && q.GetInterfaces().Any(r => r.Name == nameof(IPaymentModule)))
                    .ToList()
                    .ForEach(q =>
                    {
                        var module = Activator.CreateInstance(q) as IPaymentModule;
                        module.MerchantId = configuration.GetValue<string>($"Application:PaymentProviders:{module.BankName}:MerchantId");
                        module.Password = configuration.GetValue<string>($"Application:PaymentProviders:{module.BankName}:Password");
                        modules.Add(module.BankName, module);

                    });

                });
        }

        public Task<PaymentResult> Payment(PaymentRequest paymentRequest)
        {
            var module = modules.SingleOrDefault(p => p.Key == paymentRequest.BankName).Value ?? modules[configuration.GetValue<string>("Application:DefaultPaymentProvider")];
            return module.Payment(paymentRequest);
        }
    }
}
