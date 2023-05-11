using Microsoft.AspNetCore.Mvc;
using Moq;
using PlantsAPI.Configuration;
using PlantsAPI.Controllers;
using PlantsAPI.Models;
using PlantsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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




        /*
        #region PutReply
        [Fact]
        public void PutReply_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PutReply(null));
        }
        #endregion*/




        #region GetRepliesOfPost

        [Fact]
        public void GetRepliesOfPost_ShouldThrowArgumentNullExeption()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.GetRepliesOfPost(Guid.Empty));
        }


        [Fact]
        public void GetRepliesOfPost_ShouldReturnOk()
        {
            TestHelper helper = new();
            Reply testReply = new Reply()
            {
                Id = Guid.NewGuid(),
                DateOfCreation = DateTime.Now
            };
            IEnumerable<Reply> repliesOfPost = new List<Reply>() { testReply };

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                 x => x.Replies.GetRepliesOfPost( It.IsAny<Guid>() ))
                 .ReturnsAsync( It.IsAny<List<Reply>>);

            var response = helper.Controller.GetRepliesOfPost(Guid.NewGuid());

            Assert.True((response?.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }

        //should work after adding authorization
      /*  [Fact]
        public void GetRepliesOfPost_ShouldReturnUnAuthorized()
        {
            TestHelper helper = new();
           
            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

           
            var response = helper.Controller.GetRepliesOfPost(Guid.NewGuid());

            Assert.True((response.Result.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized); 
        }*/


        #endregion


        #region GetRepliesCount
        [Fact]
        public void GetRepliesCount_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.GetRepliesCount(Guid.Empty));
        }

        [Fact]
        public void GetRepliesCount_ShouldReturnBadRequest()
        {
            TestHelper helper = new();
            
            var response = helper.Controller.GetRepliesCount(Guid.NewGuid())?.Result;

            Assert.True((response?.Result as BadRequestResult).StatusCode == (int)HttpStatusCode.BadRequest);
        }


        [Fact]
        public void GetRepliesCount_ShouldReturnResult()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Replies.GetRepliesCount(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<int>());

            var response = helper.Controller.GetRepliesCount(Guid.NewGuid())?.Result;

            Assert.True((response?.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);

        }

        [Fact]
        public void GetRepliesCount_ShouldReturnUnauthorized()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);


            var response = helper.Controller.GetRepliesCount(Guid.NewGuid());

            Assert.NotNull(response);
            Assert.True((response.Result.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized);

        }
        #endregion

        #region PostReply
        [Fact]
        public void PostReply_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PostReply(null));
        }


        [Fact]
        public void PostReply_ShouldReturnOk()
        {
            TestHelper helper = new();
            Reply testReply = new Reply()
            {
                Id = Guid.NewGuid(),
                DateOfCreation = DateTime.Now,
                PostId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                    .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Replies.AddReply(It.IsAny<Reply>()))
                .ReturnsAsync(new Reply());

            var response = helper.Controller.PostReply(testReply);

            Assert.True((response?.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }

        [Fact]
        public void PostReply_ShouldReturnUnauthorized()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            /*helper.mockUnitOfWork.Setup(
                x => x.Replies.GetRepliesCount(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<int>());*/

            var response = helper.Controller.PostReply(new Reply());

            Assert.NotNull(response);
            Assert.True((response.Result.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized);

        }
        #endregion


        #region DeleteReply
        [Fact]
        public void DeleteReply_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.DeleteReply(Guid.Empty));
        }


        [Fact]
        public void DeleteReplyy_ShouldReturnOk()
        {
            TestHelper helper = new();
            Reply testReply = new Reply()
            {
                Id = Guid.NewGuid(),
                DateOfCreation = DateTime.Now,
                PostId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                    .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Replies.DeleteReply(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var response = helper.Controller.DeleteReply(Guid.NewGuid());

            Assert.True((response?.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
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
