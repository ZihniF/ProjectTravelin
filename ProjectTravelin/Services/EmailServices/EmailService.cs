using Microsoft.Extensions.Options;
using ProjectTravelin.Settings;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ProjectTravelin.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendBookingApprovedEmailAsync(
            string toEmail,
            string nameSurname,
            string tourName,
            string bookingDate)
        {
            var subject = "Rezervasyonunuz Onaylandı";

            var body = $@"
                <html>
                <body style='font-family:Arial, sans-serif;background:#f5f7fb;padding:30px;'>
                    <div style='max-width:620px;margin:auto;background:#ffffff;border-radius:18px;padding:30px;border:1px solid #e5e7eb;'>

                        <h2 style='color:#16a34a;margin-top:0;'>
                            Rezervasyonunuz Onaylandı
                        </h2>

                        <p>Merhaba <strong>{nameSurname}</strong>,</p>

                        <p>
                            <strong>{tourName}</strong> turu için oluşturduğunuz rezervasyon talebiniz onaylanmıştır.
                        </p>

                        <div style='background:#f8fafc;border:1px solid #e5e7eb;border-radius:14px;padding:18px;margin:22px 0;'>
                            <p style='margin:0 0 10px 0;'>
                                <strong>Tur:</strong> {tourName}
                            </p>

                            <p style='margin:0;'>
                                <strong>Tur Tarihi:</strong> {bookingDate}
                            </p>
                        </div>

                        <p>
                            Rezervasyonunuz başarıyla onaylanmıştır. Tur tarihi yaklaştığında sizinle tekrar iletişime geçilecektir.
                        </p>

                        <p style='color:#64748b;font-size:13px;margin-top:25px;'>
                            Bu e-posta Travelin rezervasyon sistemi tarafından otomatik gönderilmiştir.
                        </p>
                    </div>
                </body>
                </html>
            ";

            using var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = _emailSettings.EnableSsl
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}