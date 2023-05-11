using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;

namespace PlantsAPI.Controllers
{
    
    [Route("api/auth")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthorizationController(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.httpContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            try
            {
                string passwordSalt = unitOfWork.Auth.GenerateSalt(10);
                string passwordHash = unitOfWork.Auth.CreatePasswordHash(request.Password, passwordSalt);

                User newUser = new User();
                newUser.Name = request.Username;
                newUser.EmailAddress = request.Email;
                // newUser.ImageUrl = request.ImageUrl;
                newUser.PasswordHash = passwordHash;
                newUser.PasswordSalt = passwordSalt;

                await unitOfWork.Users.AddUser(newUser);
                await unitOfWork.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }

            //handle the case where the register is unsuccessful
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if (request == null) throw new ArgumentNullException();
            var user = await unitOfWork.Users.GetUserByName(request.Username);

            if (user == null)
            {
                return Ok(new LoginResponse(LoginStatus.UserNotFound));
            }

            string passwordHash = unitOfWork.Auth.CreatePasswordHash(request.Password, user.PasswordSalt);

            if (string.Compare(passwordHash, user.PasswordHash, true) != 0)
            {
                return Ok(new LoginResponse(LoginStatus.WrongPassword));
            }

            LoginResponse response = new()
            {
                Status = LoginStatus.Successful,
                UserId = user.Id,
                Token = unitOfWork.Auth.CreateToken(user)
            };

            return Ok(response);

        }

        [HttpGet]
        public ActionResult<string> GetMe()
        {
            return unitOfWork.UserContext.GetMe();
        }

    }
}
