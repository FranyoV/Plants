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
    public class RepliesControllerTest
    {
        #region Constructor
        [Fact]
        public void ConstructorShouldCreateObject()
        {
            TestHelper helper = new();
            Assert.NotNull(new RepliesController(helper.mockUnitOfWork.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.Throws<ArgumentNullException>(() => new RepliesController(null));
        }
        #endregion


        #region PostReply
        [Fact]
        public void PostReply_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PostReply(null));
        }
        #endregion


        #region PutReply
        [Fact]
        public void PutReply_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PutReply(null));
        }
        #endregion


        #region DeleteReply
        [Fact]
        public void DeleteReply_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.DeleteReply(Guid.Empty));
        }
        #endregion


        private class TestHelper
        {
            protected internal Mock<IReplyRepository> mockReplyRepository;
            protected internal RepliesController Controller;
            protected internal Mock<IUnitOfWork> mockUnitOfWork;

            public TestHelper()
            {
                mockReplyRepository = new Mock<IReplyRepository>();
                mockUnitOfWork = new Mock<IUnitOfWork>();
                Controller = new(mockUnitOfWork.Object);
            }
        }
    }
}
