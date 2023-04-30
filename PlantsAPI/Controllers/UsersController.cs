using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;
using PlantsAPI.Repositories;

namespace PlantsAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public UsersController(IUnitOfWork unitOFWork)
        {
            this.unitOfWork = unitOFWork;
        }

        //TODO AUTHORIZATION
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            
            var users = await unitOfWork.Users.GetUsers();
            return Ok(users);
        }

        //TODO AUTHORIZATION
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await unitOfWork.Users.GetUserById(id);
            return Ok(user);
        }

        //TODO AUTHORIZATION
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var newUser = await unitOfWork.Users.AddUser(user);
            await unitOfWork.SaveChangesAsync();
            return Ok(newUser);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<User>> PutUser(User user)
        {
            if (unitOfWork.UserContext.HasAuthorization(user.Id))
            {
                var modifiedUser = await unitOfWork.Users.EditUser(user);
                await unitOfWork.SaveChangesAsync();
                return Ok(modifiedUser);
            }
            return Unauthorized();

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<User>> DeleteUser(Guid id)
        {
            if (unitOfWork.UserContext.HasAuthorization(id))
            {
                var result = await unitOfWork.Users.DeleteUser(id);
                await unitOfWork.SaveChangesAsync();
                return Ok(result);
            }
            return Unauthorized();
        }
    }
}
