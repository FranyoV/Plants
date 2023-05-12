using System.Security.Claims;

namespace PlantsAPI.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpcontextaccesor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpcontextaccesor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string GetMe()
        {
            var result = string.Empty;
            if (_httpcontextaccesor.HttpContext != null)

            {
                
                result = _httpcontextaccesor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            }
            return result;
        }


        public bool HasAuthorization(Guid userId)
        {
            bool hasAuthorization = false;
            if (userId == Guid.Parse(GetMe()))
            {
                hasAuthorization = true;
            } 
            return hasAuthorization;
        }

        
    }
}
