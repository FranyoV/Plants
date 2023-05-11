using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsAPI.Data;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(PlantsDbContext dbContext, ILogger logger) : base(dbContext, logger) 
        { 
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await dbSet.ToListAsync();

        }

       public async Task<UserDto> GetUserById(Guid id)
        {
            var user = await dbSet.Where(u => u.Id == id).FirstOrDefaultAsync();
            var userDto = new UserDto()
            {
                Id = id,
                Name = user.Name,
                EmailAddress = user.EmailAddress,
            };
            return userDto;
        }

        public async Task<User> GetUserByName(string username)
        {
            User user = new();
            user = await dbSet.Where(u => u.Name == username).FirstOrDefaultAsync();

            UserDto userDto = new UserDto();
            if (user != null)
            {
                userDto = new UserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    EmailAddress = user.EmailAddress,
                };
            }

            return user;
        }


        //used in registration
        public async Task<User> AddUser(User user)
        {
            if (user == null) throw new NotImplementedException(nameof(user));

            var result = await dbSet.AddAsync(user);
            return result.Entity;
        }


        //TODO for profil editing
        public async Task<User> EditUser(User user)
        {
            if (user == null) throw new NotImplementedException(nameof(user));

            var originalUser = await dbSet.FirstAsync(u => u.Id == user.Id);

            if (originalUser != null)
            {
                originalUser.Name = user.Name;
                originalUser.PasswordHash = user.PasswordHash;
                     
            }

            return originalUser;
        }


        public async Task<bool> DeleteUser(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            var toBeDeleted = await dbSet.Where(u => u.Id == userId).FirstAsync();

            if (toBeDeleted != null)
            {
                var result = dbSet.Remove(toBeDeleted);
                return result.State == EntityState.Deleted;

            }
            return false;
        }


    }
}
