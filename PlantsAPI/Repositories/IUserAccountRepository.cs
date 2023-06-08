using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public interface IUserAccountRepository
    {
        string CreateToken(User user);
        string GenerateSalt(int length);
        string CreatePasswordHash(string password, string salt);

        Task<bool> UsernameTaken(string username);
        Task<UserDto> GetUserById(Guid id);
        Task<User> GetUserByName(string username);
        Task<User> AddUser(User user);
        Task<UserDto> AddImageToUser(Guid id, IFormFile image);
        Task<User> EditUserEmail(UserInfoEditRequest user);
        Task<bool> DeleteUser(Guid userId);
    }
}
