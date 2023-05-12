﻿using Microsoft.AspNetCore.Authorization;
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
            this.unitOfWork = unitOFWork ?? throw new ArgumentNullException(nameof(unitOFWork));
        }

        //TODO AUTHORIZATION
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await unitOfWork.Users.GetUsers();
                return Ok(users);
            }
            catch
            {
                return BadRequest();
            }
        }

        //TODO AUTHORIZATION
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            try
            {
                var user = await unitOfWork.Users.GetUserById(id);
                return Ok(user);
            }
            catch
            {
                return BadRequest();
            }
        }

        //TODO AUTHORIZATION
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                var newUser = await unitOfWork.Users.AddUser(user);
                await unitOfWork.SaveChangesAsync();
                return Ok(newUser);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<User>> PutUser(User user)
        {
            try
            {
 
                    var modifiedUser = await unitOfWork.Users.EditUser(user);
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
  
                    var result = await unitOfWork.Users.DeleteUser(id);
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
