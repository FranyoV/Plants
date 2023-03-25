using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;
using PlantsAPI.Repositories;

namespace PlantsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public UsersController(IUnitOfWork unitOFWork)
        {
            this.unitOfWork = unitOFWork;
        }

        [HttpGet]
        [Route("users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            
            var users = await unitOfWork.Users.GetUsers();
            return Ok(users);
        }

        [HttpGet]
        [Route("user")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await unitOfWork.Users.GetUserById(id);
            return Ok(user);
        }

        [HttpPost]
        [Route("user")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var newUser = await unitOfWork.Users.AddUser(user);
            await unitOfWork.SaveChangesAsync();
            return Ok(newUser);
        }

        [HttpPut]
        [Route("user")]
        public async Task<ActionResult<User>> PutUser(User user)
        {
            var modifiedUser = await unitOfWork.Users.EditUser(user);
            await unitOfWork.SaveChangesAsync();
            return Ok(modifiedUser);
        }

        [HttpDelete]
        [Route("user")]
        public async Task<ActionResult<User>> DeleteUser(Guid id)
        {
            var result = await unitOfWork.Users.DeleteUser(id);
            await unitOfWork.SaveChangesAsync();
            return Ok(result);
        }
    }
}
