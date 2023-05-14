using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public interface IAuthRepository
    {
        string CreateToken(User user);
        string GenerateSalt(int length);
        string CreatePasswordHash(string password, string salt);

        Task<UserDto> GetUserById(Guid id);
        Task<User> GetUserByName(string username);
        Task<User> AddUser(User user);
        Task<User> EditUserEmail(UserInfoEditRequest user);
        Task<bool> DeleteUser(Guid userId);
    }
}
