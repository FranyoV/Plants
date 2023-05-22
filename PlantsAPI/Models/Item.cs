using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlantsAPI.Models
{
    public class Item
    {
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public bool Sold { get; set; }


        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
