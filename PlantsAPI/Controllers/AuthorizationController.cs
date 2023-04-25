﻿using Microsoft.AspNetCore.Mvc;
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
            this.unitOfWork = unitOfWork;   
            this.httpContextAccessor = contextAccessor;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Register([FromBody] LoginRequest request)
        {
            string passwordSalt = unitOfWork.Auth.GenerateSalt(10);
            string passwordHash = unitOfWork.Auth.CreatePasswordHash(request.Password, passwordSalt);
            
            User newUser = new User();
            newUser.Name = request.Username;
            newUser.EmailAddress = request.EmailAddress;
           // newUser.ImageUrl = request.ImageUrl;
            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;

            await unitOfWork.Users.AddUser(newUser);
            await unitOfWork.SaveChangesAsync();

            return Ok(newUser);
            //handle the case where the register is unsuccessful
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var user = unitOfWork.Users.GetUserByName(request.Username).Result;

            if (user == null)
            {
                return Ok(new LoginResponse(LoginStatus.UserNotFound));
            }

            string passwordHash = unitOfWork.Auth.CreatePasswordHash(request.Password, user.PasswordSalt);

            if ( string.Compare(passwordHash, user.PasswordHash, true) != 0)
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

    }
}