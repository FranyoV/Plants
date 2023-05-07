using PlantsAPI.Configuration;
using PlantsAPI.Models;
using PlantsAPI.Services;
using Quartz;

namespace PlantsAPI.Jobs
{
    public class SendMaintenanceEmailJob : IJob
    {
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;

        public SendMaintenanceEmailJob(INotificationService notificationService, IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await SendMaintenanceEmail();
        }

        public async Task SendMaintenanceEmail()
        {
            var allPlants = await _unitOfWork.Plants.GetPlants();

            foreach (var plant in allPlants)
            {
                if (plant.LastNotification != null)
                {
                    if (plant.LastNotification.Value.AddDays((double)plant.Interval) == DateTime.Today.Date)
                    {
                        EmailData emailData = new()
                        {
                            DataName = plant.Name,
                            Subject = "Maintenance",
                            Body = (plant.Note != null) ? plant.Note : "",
                            Url = "http://localhost:4200/plants/" + plant.Id,
                        };

                        _notificationService.SendEmail(emailData, EmailTemplate.MAINTENANCE);

                    }
                }
            }
        }
    }

}
