using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MatrimonyApiService.Commons.Services;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string recipientEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Matrimony Service Desk", "yenbinmaster@gmail.com"));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using (var smtpClient = new SmtpClient())
        {
            smtpClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtpClient.Authenticate("yenbinmaster@gmail.com", _configuration["Email:AppPassword"]);

            smtpClient.Send(message);
            smtpClient.Disconnect(true);
        }
    }
}
