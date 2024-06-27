using System.Collections.Concurrent;
using System.Timers;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using OtpNet;
using Timer = System.Timers.Timer;

namespace MatrimonyApiService.Auth;

public class OtpService
{
    private static readonly ConcurrentDictionary<string, OtpEntry> OtpStorage = new();
    private readonly Timer _cleanupTimer;
    private readonly IConfiguration _configuration;

    public OtpService(IConfiguration configuration)
    {

        _configuration = configuration;
        _cleanupTimer = new Timer(60000); 
        _cleanupTimer.Elapsed += CleanupExpiredOtps;
        _cleanupTimer.Start();
    }

    public string GenerateAndSendOtp(string email)
    {
        var otp = GenerateOtp();
        var expiryTime = DateTime.UtcNow.AddMinutes(5); // OTP valid for 5 minutes

        OtpStorage[email] = new OtpEntry { Otp = otp, ExpiryTime = expiryTime };

        SendEmail(email, otp);
        return otp;
    }

    public bool VerifyOtp(string email, string enteredOtp)
    {
        if (OtpStorage.TryGetValue(email, out var otpEntry))
        {
            if (otpEntry.ExpiryTime > DateTime.UtcNow && otpEntry.Otp == enteredOtp)
            {
                OtpStorage.TryRemove(email, out _);
                return true;
            }
        }
        return false;
    }

    private void CleanupExpiredOtps(object sender, ElapsedEventArgs e)
    {
        foreach (var kvp in OtpStorage)
        {
            if (kvp.Value.ExpiryTime <= DateTime.UtcNow)
            {
                OtpStorage.TryRemove(kvp.Key, out _);
            }
        }
    }

    private string GenerateOtp()
    {
        var key = KeyGeneration.GenerateRandomKey(20);
        var totp = new Totp(key);
        return totp.ComputeTotp();
    }

    private void SendEmail(string email, string otp)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Matrimony Service Desk", "yenbinmaster@gmail.com"));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "OTP for Account verification";

        message.Body = new TextPart("plain")
        {
            Text = $"Your OTP code is {otp}. It is valid for 5 minutes."
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