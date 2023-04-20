using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlantsAPI.Models
{
    public class Plant
    {
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

       
        public string? Description { get; set; } 
        public string? ImageUrl { get; set; }

        public string? Note { get; set; }
        public int? Interval { get; set; }
        public DateTime? LastNotification { get; set; }
        public DateTime? NextNotification { get; set; }

        public Guid UserId { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
