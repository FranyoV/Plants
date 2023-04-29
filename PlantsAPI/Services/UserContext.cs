using System.Security.Claims;

namespace PlantsAPI.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpcontextaccesor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpcontextaccesor = httpContextAccessor;
        }

        public string GetMe()
        {
            var result = string.Empty;
            if (_httpcontextaccesor.HttpContext != null)

            {
                var claims = _httpcontextaccesor.HttpContext.User.Claims.ToList();
                result = _httpcontextaccesor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);


            }
            return result;
        }
    }
}
