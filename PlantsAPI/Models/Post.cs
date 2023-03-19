namespace PlantsAPI.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content   { get; set; }
        public DateTime DateOfCreation { get; set; }   

        public virtual User User { get; set; }
        public virtual IEnumerable<Reply> Replies { get; set; }
    }
}
