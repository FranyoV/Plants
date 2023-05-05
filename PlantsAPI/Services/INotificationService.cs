using PlantsAPI.Models;

namespace PlantsAPI.Services
{
    public interface INotificationService
    {
        void SendEmail(EmailData emailData, EmailTemplate template);
    }
}
