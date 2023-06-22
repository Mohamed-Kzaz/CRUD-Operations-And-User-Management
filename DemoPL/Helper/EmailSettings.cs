using Demo.DAL.Entities;
using System.Net;
using System.Net.Mail;

namespace DemoPL.Helper
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            //Host
            var client = new SmtpClient("smtp.gmail.com", 587);

            client.EnableSsl= true;


            //Add The Account From Which The Email Will Be Sent After Activation 2-Step Verification In Account
            client.Credentials = new NetworkCredential(" ", " ");

            client.Send(" ", email.To, email.Title, email.Body);
        }
    }
}
