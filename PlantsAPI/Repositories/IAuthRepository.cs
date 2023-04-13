using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public interface IAuthRepository
    {
        string CreateToken(User user);
        string GenerateSalt(int length);
        string CreatePasswordHash(string password, string salt);
    }
}
