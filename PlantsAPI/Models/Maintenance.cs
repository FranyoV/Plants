using System.Text.Json.Serialization;

namespace PlantsAPI.Models
{
    public class Maintenance
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Note { get; set; }
        public int Interval { get; set; }
        public DateTime LastNotification { get; set; }
        public DateTime NextNotification { get { return NextNotification; } set { LastNotification.AddDays(Interval); } }  
        
        [JsonIgnore]
        public virtual Plant Plant { get; set; }
        //public string EmailAdress { get; set; }

        //Template Email with the data
    }
}
