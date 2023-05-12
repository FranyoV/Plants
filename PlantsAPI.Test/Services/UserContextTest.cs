using Microsoft.AspNetCore.Http;
using Moq;
using PlantsAPI.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace PlantsAPI.Test.Services
{
    public class UserContextTest
    {
        private readonly Mock<IUserContext> userContext;
        private readonly Mock<IHttpContextAccessor> httpContextAccesor;

        public UserContextTest()
        {
            httpContextAccesor = new Mock<IHttpContextAccessor>();
        }

        [Fact]
        public void ContructorShouldCreateObject()
        {
            Assert.NotNull(new UserContext(httpContextAccesor.Object));
        }

        [Fact]
        public void Contructor_ShouldThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new UserContext(null));
        }

        [Fact]
        public void GetMe_ShouldReturnResult()
        {
            httpContextAccesor.Setup(
                x => x.HttpContext.User.FindFirstValue(It.IsAny<string>()))
                .Returns(It.IsAny<string>());

            
        }

    }
}

