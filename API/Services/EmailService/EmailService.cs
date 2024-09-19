/* using API.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

		private readonly EmailSmtpSettings _smtpSettings;

		public EmailService(IConfiguration configuration, IOptions<EmailSmtpSettings> smtpSettings)
        {
            _configuration = configuration;
			_smtpSettings = smtpSettings.Value;

		}

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient(_configuration["EmailSmtpSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSmtpSettings:Port"]),
                Credentials = new NetworkCredential(_configuration["EmailSmtpSettings:Username"], _configuration["EmailSmtpSettings:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSmtpSettings:SenderEmail"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }

    }
} */


using API.Entities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Services.EmailService
{
	public class EmailService : IEmailService
	{
		private readonly EmailSmtpSettings _smtpSettings;

		public EmailService(IOptions<EmailSmtpSettings> smtpSettings)
		{
			_smtpSettings = smtpSettings.Value;
		}

		public async Task SendEmailAsync(string to, string subject, string body)
		{
			using (var smtpClient = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
			{
				smtpClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
				smtpClient.EnableSsl = _smtpSettings.UseSSL;

				var mailMessage = new MailMessage
				{
					From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
					Subject = subject,
					Body = body,
					IsBodyHtml = true,
				};

				mailMessage.To.Add(to);

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to send email: {ex.Message}", ex);
                }
            }
		}


        public async Task SendCandidateRegistrationConfirmationAsync(string email, string firstName)
        {
            var subject = "Registration Successful - Welcome!";
            var message = $@"
            <h3>Dear {firstName},</h3>
            <p>Congratulations! Your registration has been successfully completed.</p>
            <p>We are excited to have you on board.</p>
            <br/>
            <p>Best regards,</p>
            <p>First Academy</p>";

            await SendEmailAsync(email, subject, message);
        }

    }
}
