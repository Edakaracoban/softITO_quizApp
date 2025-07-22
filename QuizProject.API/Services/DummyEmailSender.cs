using System;
using System.Threading.Tasks;

namespace QuizProject.API.Services
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string body)
        {
            Console.WriteLine($"📧 E-mail to: {toEmail}");
            Console.WriteLine($"📌 Subject: {subject}");
            Console.WriteLine($"📄 Body: {body}");
            return Task.CompletedTask;
        }
    }
}