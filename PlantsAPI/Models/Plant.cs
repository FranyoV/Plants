using System.Text.Json.Serialization;

namespace PlantsAPI.Models
{
    public class Plant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } 
        public string? ImageUrl { get; set; }  

        //public Maintenance Maintenance { get; set; }
        
        public Guid UserId { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
