using DShopAPI.Interfaces;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DShopAPI.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public SmtpEmailService(IConfiguration configuration)
        {
            var smtpSettings = configuration.GetSection("SmtpSettings");
            _smtpServer = smtpSettings.GetValue<string>("Server");
            _smtpPort = smtpSettings.GetValue<int>("Port");
            _smtpUsername = smtpSettings.GetValue<string>("Username");
            _smtpPassword = smtpSettings.GetValue<string>("Password");
        }


        public async Task<bool> SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(_smtpUsername);
                    message.To.Add(email);
                    message.Subject = subject;
                    message.Body = body;

                    using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                        smtpClient.EnableSsl = true;

                        await smtpClient.SendMailAsync(message);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception
                return false;
            }
        }
    }
}
