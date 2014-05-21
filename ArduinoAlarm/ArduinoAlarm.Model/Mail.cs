using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ArduinoAlarm.Model
{
    public class Mail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }



        public async Task Send()
        {
            using (var message = new MailMessage { Body = Body, From = new MailAddress(From), Subject = Subject })
            {
                message.To.Add(To);

                using (var smtp = new SmtpClient("smtp.gmail.com") { EnableSsl = true, Credentials = new NetworkCredential("dupuispbaptiste@gmail.com", "1234abcdE") })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
    }
}
