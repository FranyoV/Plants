using System.Text.Json.Serialization;

namespace PlantsAPI.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //public string PasswordHash { get; set; }

        // public string EmailAddress { get; set; }    

    }
}
