using Microsoft.IdentityModel.Tokens;
using PlantsAPI.Data;
using PlantsAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PlantsAPI.Repositories
{
    public class AuthRepository : GenericRepository<User>, IAuthRepository
    {
        private readonly IConfiguration configuration;

        public AuthRepository(
            PlantsDbContext dbContext,
            ILogger logger,
            IConfiguration configuration) : base(dbContext, logger)
        {
            this.configuration = configuration;
        }


        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("Appsettings:SymmetricKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

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
                    hashBuilder.AppendFormat("{§:X2}", currentByte);
                }

                return hashBuilder.ToString();
            }
        }



        public string GenerateSalt(int length)
        {
            StringBuilder saltBuilder = new StringBuilder();

            string saltCharacters = "abcdefghijklmnopqrstuvwxzyABCDEFGHIJKLMNOPQRSTUVWXZY0123456789";
            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < length; i++)
            {
                int nextCharIndex = rnd.Next(saltCharacters.Length - 1);
                saltBuilder.Append(saltCharacters[nextCharIndex]);
            }

            return saltBuilder.ToString();
        }
    }
}
