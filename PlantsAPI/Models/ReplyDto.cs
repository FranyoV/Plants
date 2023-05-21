namespace PlantsAPI.Models
{
    public class ReplyDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime DateOfCreation { get; set; }

        public string Username { get; set; }

        public Guid PostId { get; set; }

        public Guid UserId { get; set; }

    }
}
