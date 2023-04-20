using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlantsAPI.Models
{
    public class Post
    {
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }
        public string Content   { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string ImageUrl { get; set; }

        //public virtual UserDto? UserDto { get; set; }
        public Guid UserId { get; set; }


        [JsonIgnore]
        public virtual User? User { get; set; }

        [JsonIgnore]
        public IEnumerable<Reply>? Replies { get; set; }        
    }
}
