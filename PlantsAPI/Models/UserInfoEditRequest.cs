namespace PlantsAPI.Models
{
    public class UserInfoEditRequest
    {
        public Guid UserId { get; set; }
        public string UserInfo { get; set; }
        public string Password { get; set; }
    }
}
