using System.Text.Json.Serialization;

namespace PlantsAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string? ImageUrl { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }


        [JsonIgnore]
        public IEnumerable<Plant>? Plants { get; set; }

        [JsonIgnore]
        public IEnumerable<Post>? Posts { get; set; }

        [JsonIgnore]
        public IEnumerable<Reply>? Replies { get; set; }
    }
}
