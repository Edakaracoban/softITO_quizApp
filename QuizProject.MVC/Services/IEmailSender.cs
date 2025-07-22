using System.Threading.Tasks;

namespace QuizProject.MVC.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
