using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;

namespace PlantsAPI.Controllers
{
    
    [Route("api/account")]
   // [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public UserAccountController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            try
            {
                if (await unitOfWork.UserAccounts.UsernameTaken(request.Username))
                {
                    return Ok(new RegisterResponse() { state = RegisterStatus.USERNAMETAKEN});
                }

                string passwordSalt = unitOfWork.UserAccounts.GenerateSalt(10);
                string passwordHash = unitOfWork.UserAccounts.CreatePasswordHash(request.Password, passwordSalt);

                User newUser = new();
                newUser.Name = request.Username;
                newUser.EmailAddress = request.Email;
                newUser.PasswordHash = passwordHash;
                newUser.PasswordSalt = passwordSalt;

                await unitOfWork.UserAccounts.AddUser(newUser);
                await unitOfWork.SaveChangesAsync();

                return Ok(new RegisterResponse() { state = RegisterStatus.SUCCESSFULL });
            }
            catch
            {
                return BadRequest(new RegisterResponse() { state = RegisterStatus.UNSUCCESSFULL });
            }
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                var user = await unitOfWork.UserAccounts.GetUserByName(request.Username);

                if (user == null)
                {
                    return Ok(new LoginResponse(LoginStatus.UserNotFound));
                }

                string passwordHash = unitOfWork.UserAccounts.CreatePasswordHash(request.Password, user.PasswordSalt);

                if (string.Compare(passwordHash, user.PasswordHash, true) != 0)
                {
                    return Ok(new LoginResponse(LoginStatus.WrongPassword));
                }

                LoginResponse response = new()
                {
                    Status = LoginStatus.Successfull,
                    UserId = user.Id,
                    Token = unitOfWork.UserAccounts.CreateToken(user)
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
                var user = await unitOfWork.UserAccounts.GetUserById(id);
                return Ok(user);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("{userId}/image")]
        public async Task<ActionResult<UserDto>> AddImageToUser([FromForm]IFormFile image, [FromRoute] Guid userId)
        {
            try
            {
                UserDto modifiedUser = new();
                modifiedUser = await unitOfWork.UserAccounts.AddImageToUser(userId, image);
                await unitOfWork.SaveChangesAsync();
                return Ok(modifiedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }



        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<UserDto>> EditUser(UserInfoEditRequest request)
        {
            try
            {
                var modifiedUser = await unitOfWork.UserAccounts.EditUserEmail(request);
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
        public async Task<ActionResult<bool>> DeleteUser(Guid id)
        {
            try
            {
                var result = await unitOfWork.UserAccounts.DeleteUser(id);
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
