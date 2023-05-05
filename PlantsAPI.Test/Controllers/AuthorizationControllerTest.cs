using Microsoft.AspNetCore.Http;
using Moq;
using PlantsAPI.Configuration;
using PlantsAPI.Controllers;
using PlantsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PlantsAPI.Test.Controllers
{
    public class AuthorizationControllerTest
    {
        #region Constructor
        [Fact]
        public void ConstructorShouldCreateObject()
        {
            TestHelper helper = new();
            Assert.NotNull(new AuthorizationController(helper.mockUnitOfWork.Object, helper.mockHttpContextAccesor.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_1()
        {
            TestHelper helper = new();
            Assert.Throws<ArgumentNullException>(() => new AuthorizationController(null, helper.mockHttpContextAccesor.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_2()
        {
            TestHelper helper = new();
            Assert.Throws<ArgumentNullException>(() => new AuthorizationController(helper.mockUnitOfWork.Object, null));
        }
        #endregion


        private class TestHelper
        {
            protected internal Mock<IAuthRepository> mockAuthRepository;
            protected internal AuthorizationController Controller;
            protected internal Mock<IUnitOfWork> mockUnitOfWork;
            protected internal Mock<IHttpContextAccessor> mockHttpContextAccesor;

            public TestHelper()
            {
                mockAuthRepository = new Mock<IAuthRepository>();
                mockUnitOfWork = new Mock<IUnitOfWork>();
                mockHttpContextAccesor = new Mock<IHttpContextAccessor>();
                Controller = new(mockUnitOfWork.Object, mockHttpContextAccesor.Object);
            }
        }
    }
}
