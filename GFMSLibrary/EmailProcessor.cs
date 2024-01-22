using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GFMSLibrary
{
    public static class EmailProcessor
    {
        public static async Task<bool> SendEmail(string RecipientEmail, string otp)
        {
            try
            {
                string smtpServer = "smtp.gmail.com"; // Specify your SMTP server
                int smtpPort = 587; // Specify the SMTP server port (e.g., 587 for TLS)
                string senderEmail = "fanchannel13@gmail.com"; // Your email address
                string senderPassword = "sbfjzjnkbxgshqow"; // Your email password

                using (SmtpClient client = new SmtpClient(smtpServer))
                {
                    client.Port = smtpPort;
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    client.EnableSsl = true; // Use SSL/TLS if supported by the SMTP server

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(senderEmail);
                        mail.To.Add(RecipientEmail);
                        mail.Subject = "Forgot Password";
                        mail.Body = "To continue to change your password, please enter the following OTP into the program. Thank you.<br><br><h1>" + otp + "</h1>";
                        mail.IsBodyHtml = true;

                        await client.SendMailAsync(mail); // Use SendMailAsync for async operation
                    }
                }
                Debug.WriteLine("Email sent successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }
}
