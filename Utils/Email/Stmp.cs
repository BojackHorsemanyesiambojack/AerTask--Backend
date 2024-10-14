using System.Net;
using System.Net.Mail;

namespace AerTaskAPI.Utils.Email
{
    public class Stmp
    {
        public static SmtpClient GenerateDefaultApiSmtp(string Email, string Password)
        {
            return new SmtpClient
            {
                Host = "smtp.gmail.com ",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Email, Password)
            };
        }
    }
}
