namespace PlantsAPI.Models
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public byte[]? ImageData { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public string Email { get;set; }
        public Guid UserId { get; set; }
        public bool Sold { get; set; }

    }
}
