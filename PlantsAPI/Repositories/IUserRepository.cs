using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(Guid id);
        Task<User> AddUser(User user);
        Task<User> EditUser(User user);
        Task<bool> DeleteUser(Guid userId);
        Task<User> GetUserByName(string username);
    }
}
