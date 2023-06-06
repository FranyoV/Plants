using System.Text.Json.Serialization;

namespace PlantsAPI.Models
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string? ImageUrl { get; set; }

        //userdata
        public string UserName { get; set; }
        public int? ReplyCount { get; set; }
        public Guid UserId { get; set; }


    }
}
