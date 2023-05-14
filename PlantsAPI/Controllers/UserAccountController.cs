using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;

namespace PlantsAPI.Controllers
{
    
    [Route("api/account")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public UserAccountController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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

                User newUser = new();
                newUser.Name = request.Username;
                newUser.EmailAddress = request.Email;
                // newUser.ImageUrl = request.ImageUrl;
                newUser.PasswordHash = passwordHash;
                newUser.PasswordSalt = passwordSalt;

                await unitOfWork.Auth.AddUser(newUser);
                await unitOfWork.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                var user = await unitOfWork.Auth.GetUserByName(request.Username);

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
            catch
            {
                return BadRequest();
            }
        }



        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            try
            {
                var user = await unitOfWork.Auth.GetUserById(id);
                return Ok(user);
            }
            catch
            {
                return BadRequest();
            }
        }



        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<User>> EditUser(UserInfoEditRequest request)
        {
            try
            {
                var modifiedUser = await unitOfWork.Auth.EditUserEmail(request);
                await unitOfWork.SaveChangesAsync();
                return Ok(modifiedUser);

            }
            catch
            {
                return BadRequest();
            }

        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<User>> DeleteUser(Guid id)
        {
            try
            {
                var result = await unitOfWork.Auth.DeleteUser(id);
                await unitOfWork.SaveChangesAsync();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
