namespace PlantsAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public IEnumerable<Plants> Plants { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Reply> Replies { get; set; }
    }
}
