using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FormBot.Services
{
    public class SmtpEmailSender
    {
        public static async Task<bool> SendEmailAsync(string from, string to, string subject, string body)
        {
            try
            {
                string userName = ConfigurationManager.AppSettings["EmailUserName"].ToString();
                string passWord = ConfigurationManager.AppSettings["EmailPassword"].ToString();

                NetworkCredential credentials = new NetworkCredential(userName, passWord);

                using (var client = SmtpClientFactory.GetClient(SmtpClientFactory.ClientType.Gmail, credentials, true))
                {
                    MailMessage message = new MailMessage(from, to, subject, body);

                    await client.Send(message);

                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}