using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlantsAPI.Models
{
    public class Reply
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime DateOfCreation { get; set; }

        [ForeignKey("Post")]
        public Guid PostId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public virtual Post? Post { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
