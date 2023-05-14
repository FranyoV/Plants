using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
       // Task<UserDto> GetUserById(Guid id);
        //Task<User> AddUser(User user);
       // Task<User> EditUserEmail(UserInfoEditRequest user, string hash);
        //Task<bool> DeleteUser(Guid userId);
        //Task<User> GetUserByName(string username);
    }
}
