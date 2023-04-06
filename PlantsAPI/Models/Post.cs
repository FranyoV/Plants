using System.Text.Json.Serialization;

namespace PlantsAPI.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content   { get; set; }
        public DateTime DateOfCreation { get; set; }

       // public UserDto UserDto { get; set; }
        public Guid UserId { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }

        [JsonIgnore]
        public IEnumerable<Reply>? Replies { get; set; }        
    }
}
