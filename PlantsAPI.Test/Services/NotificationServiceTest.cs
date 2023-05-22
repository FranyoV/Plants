using Microsoft.Extensions.Configuration;
using Moq;
using PlantsAPI.Services;
using System;
using Xunit;

namespace PlantsAPI.Test.Services
{
    public class NotificationServiceTest
    {
        private readonly Mock<IConfiguration> configuration;

        public NotificationServiceTest()
        {
            configuration = new Mock<IConfiguration>();
        }

        #region Constructor
        [Fact]
        public void ContructorShouldCreateObject()
        {
            Assert.NotNull(new NotificationService(configuration.Object));
        }

        [Fact]
        public void Contructor_ShouldThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new NotificationService(null));
        }
        #endregion
    }
}
