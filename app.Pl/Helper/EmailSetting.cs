using app.DAL.model;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace app.Pl.Helper
{
    public static class EmailSetting
    {

        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("muhammad.m.moustafaa@gmail.comProfile image", "rgzeexicccjvjnz");
            client.Send("muhammad.m.moustafaa@gmail.comProfile image", email.To, email.Subject, email.Body);
        }
    }

    //public class EmailSetting
    //{
    //    private readonly EmailSettings _settings;

    //    public EmailSetting(IOptions<EmailSettings> options)
    //    {
    //        _settings = options.Value;
    //    }

    //    public void SendEmail(Email email)
    //    {
    //        var client = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort);
    //        client.EnableSsl = _settings.EnableSSL;
    //        client.Credentials = new NetworkCredential(_settings.Email, _settings.Password);

    //        client.Send(_settings.Email, email.To, email.Subject, email.Body);
    //    }
    //}

}
