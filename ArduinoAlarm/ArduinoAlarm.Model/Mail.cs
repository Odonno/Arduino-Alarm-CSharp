using System.Net;
using System.Net.Mail;

namespace ArduinoAlarm.Model
{
    public class Mail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }



        public void Send()
        {
            using (var message = new MailMessage { Body = Body, From = new MailAddress(From), Subject = Subject })
            {
                message.To.Add(To);

                using (var smtp = new SmtpClient("smtp.laposte.net") { Credentials = new NetworkCredential("username", "password") })
                {
                    smtp.Send(message);
                    //smtp.SendAsync(message, null);
                }
            }
        }
    }
}
