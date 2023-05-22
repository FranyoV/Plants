namespace PlantsAPI.Models
{
    public class LoginResponse
    {
        public LoginStatus Status { get; set; }
        public Guid? UserId { get; set; }
        public string Token { get; set; }
        public LoginResponse()
        {

        }

        public LoginResponse( LoginStatus status)
        {
            this.Status = status;
        }

        public LoginResponse(LoginStatus status, Guid userId)
        {
            this.Status = status;
            this.UserId = userId;
        }
    }
}
