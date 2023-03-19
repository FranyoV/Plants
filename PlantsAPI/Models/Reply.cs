namespace PlantsAPI.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateOfCreation { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
