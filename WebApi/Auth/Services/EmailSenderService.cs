using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Mail;
using WebApi.Models;

namespace WebApi.Auth.Services
{
    public class EmailSenderService : IEmailSender<ApplicationUser>
    {
        public async Task SendMessage(string emailSender, string password, string email, string subject, string message, int port, string domain)
        {
            var mail = emailSender;
            var passwd = password;
            ;
            var client = new SmtpClient(domain, port)
            {
                EnableSsl = true,

                Credentials = new NetworkCredential(mail, passwd)
            };

            await client.SendMailAsync(new MailMessage
                (
                    from: email,
                    to: mail,
                    subject: subject,
                    body: message
                ));
        }
        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }
    }
}
