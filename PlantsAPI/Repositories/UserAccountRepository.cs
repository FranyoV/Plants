using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PlantsAPI.Repositories
{
    public class UserAccountRepository : GenericRepository<User>, IUserAccountRepository
    {
        private readonly IConfiguration configuration;

        public UserAccountRepository(PlantsDbContext dbContext, IConfiguration configuration, IUserContext userContext) : base(dbContext, userContext)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                //new Claim(ClaimTypes.Role, "user")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("Appsettings:SymmetricKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            //add issuer and other properties
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public string CreatePasswordHash(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordHashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));

                var hashBuilder = new StringBuilder();
                foreach (var currentByte in passwordHashBytes)
                {
                    hashBuilder.AppendFormat("{0:X2}", currentByte);
                }
               
                return hashBuilder.ToString();
            }
        }



        public string GenerateSalt(int length)
        {
            StringBuilder saltBuilder = new();

            string saltCharacters = "abcdefghijklmnopqrstuvwxzyABCDEFGHIJKLMNOPQRSTUVWXZY0123456789";
            Random rnd = new(DateTime.Now.Millisecond);

            for (int i = 0; i < length; i++)
            {
                int nextCharIndex = rnd.Next(saltCharacters.Length - 1);
                saltBuilder.Append(saltCharacters[nextCharIndex]);
            }

            return saltBuilder.ToString();
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


        public async Task<bool> UsernameTaken(string username)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));

            if (await dbSet.Where(u => u.Name == username).AnyAsync())
            {
                return true;
            }
            else { return false; }
        }


        public async Task<User> GetUserByName(string username)
        {
            User user = new();
            user = await dbSet.Where(u => u.Name == username).FirstOrDefaultAsync();

            UserDto userDto = new();
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

        //should return bool
        //used in registration
        public async Task<User> AddUser(User user)
        {
            if (user == null) throw new NotImplementedException(nameof(user));

            var result = await dbSet.AddAsync(user);
            return result.Entity;
        }


        public async Task<UserDto> AddImageToUser(Guid id, IFormFile image)
        {
            var planty = image;
            byte[] imageByteArray = null;
            using (var readStream = image.OpenReadStream())
            using (var memoryStream = new MemoryStream())
            {
                readStream.CopyTo(memoryStream);
                imageByteArray = memoryStream.ToArray();
            }
            var todb = imageByteArray;

            User user = new();
            UserDto userDto = new();

            if (todb != null)
            {
                user = await dbSet.Where(p => p.Id == id).FirstAsync();
                user.ImageData = todb;
                userDto.Name = user.Name;
                userDto.Id = user.Id;
                userDto.EmailAddress = user.EmailAddress;
                userDto.ImageData = user.ImageData;
            }
            return userDto;
        }

        //SHOULD RETURN USERdTO
        //TODO for profil editing
        public async Task<User> EditUserEmail(UserInfoEditRequest request)
        {
            if (request == null) throw new NotImplementedException(nameof(request));

            if (_userContext.HasAuthorization(request.UserId))
            {

                var originalUser = await dbSet.FirstAsync(u => u.Id == request.UserId);

                string compareHash = CreatePasswordHash(request.Password, originalUser.PasswordSalt);

                if (originalUser != null && (originalUser.PasswordHash == compareHash))
                {

                    originalUser.EmailAddress = request.UserInfo;
                }

                return originalUser;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
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
