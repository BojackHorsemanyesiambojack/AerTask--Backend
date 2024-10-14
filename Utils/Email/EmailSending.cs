using System.Net.Mail;
using System.Net;
using AerTaskAPI.Shared.Methods;

namespace AerTaskAPI.Utils.Email
{
    public class EmailSending
    {
        public static MailAddress OriginEmail { get; set; } = new MailAddress("aerotaskclient@gmail.com","MailAdress");

        public static void GenerateEmailVerification(string DestinyEmail, string Token)
        {
            try
            {
                var ToAddress = new MailAddress(DestinyEmail, "Destinary");
                const string Title = "Email verification - AerTask";
                string body = $"<h1>Verificacion de correo</h1><p>Su codigo de verificacion es " +
                    $"<strong>{Token}</strong>.</p>";
                string EmailPassword = GetEnv.GetEmailPassword();
                var smtp =
                Stmp.GenerateDefaultApiSmtp(OriginEmail.Address, EmailPassword);
                using (var message = new MailMessage(OriginEmail, ToAddress)
                {
                    Subject = Title,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
            }catch(OperationCanceledException ex)
            {
                throw new Exception("Error sending email verification");
            }

        }
}
}
