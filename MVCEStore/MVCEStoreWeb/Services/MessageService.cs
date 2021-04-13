using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace MVCEStoreWeb.Services
{
    public class MessageService : IMessageService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration configuration;

        public MessageService(
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration
            )
        {
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
        }

        public async Task SendEmailConfirmationMessage(string to, string name, string link)
        {
            var body = string.Format(
                File.ReadAllText(Path.Combine(webHostEnvironment.WebRootPath, "content", "messagetemplates", "emailconfirmation.html")),
                name,
                link
                );

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("MVCEStore", configuration.GetValue<string>("Application:Smtp:Account")));
            message.To.Add(new MailboxAddress(name, to));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(configuration.GetValue<string>("Application:Smtp:Host"), configuration.GetValue<int>("Application:Smtp:Port"), configuration.GetValue<bool>("Application:Smtp:IsSslEnabled"));
                await client.AuthenticateAsync(configuration.GetValue<string>("Application:Smtp:UserName"), configuration.GetValue<string>("Application:Smtp:Password"));
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
