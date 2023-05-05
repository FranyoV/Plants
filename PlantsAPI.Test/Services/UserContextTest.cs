using Microsoft.AspNetCore.Http;
using Moq;
using PlantsAPI.Services;
using System;
using Xunit;

namespace PlantsAPI.Test.Services
{
    public class UserContextTest
    {
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

    }
}

