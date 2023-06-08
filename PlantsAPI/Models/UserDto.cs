using System.Text.Json.Serialization;

namespace PlantsAPI.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public byte[] ImageData { get; set; }
        public string EmailAddress { get; set; }    

    }
}
