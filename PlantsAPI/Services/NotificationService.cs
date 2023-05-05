using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using PlantsAPI.Models;

namespace PlantsAPI.Services
{
    public class NotificationService : INotificationService
    {

        private readonly IConfiguration _configuration;

        public NotificationService(IConfiguration config)
        {
            _configuration = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void SendEmail(EmailData emailData, EmailTemplate template )
        {
            
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse("ryann.rempel@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(emailData.Recipicent));

            if(template == EmailTemplate.NEWREPLY)
            {
                email.Subject = "New Reply";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = "<p> Heey! <br><br>" +
                    "Someone left a comment on your post. <br>" +
                    "Check it out on the website:<a>" + emailData.Url + "</a> . <br>" +
                    "Have a great day!</p> "
                };
            }else if(template == EmailTemplate.MAINTENANCE)
            {
                email.Subject = "Maintenance";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = "<p> Heey! <br><br>" +
                    "Don't forget to take care of your plant! <br>" +
                    "Here's your note for <i>" + emailData.DataName + "'s</i> maintenance:" + emailData.Url + ".</p> "
                };
            }

            
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("ryann.rempel@ethereal.email", "wuuECj5zkww3maSeNr");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
