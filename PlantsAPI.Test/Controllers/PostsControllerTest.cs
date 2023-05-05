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

namespace PlantsAPI.Test
{
    public class PostsControllerTest
    {
        #region Constructor
        [Fact]
        public void ConstructorShouldCreateObject()
        {
            TestHelper helper = new();
            Assert.NotNull(new PostsController(helper.mockUnitOfWork.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.Throws<ArgumentNullException>(() => new PostsController(null));
        }
        #endregion


        #region GetPostByUser
        [Fact]
        public void GetPostByUser_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.GetPostByUser(Guid.Empty));
        }


        [Fact]
        public void GetPostByUser_ShouldReturnResult()
        {
            TestHelper helper = new();

            PostDto post = new PostDto()
            {
                Id = Guid.NewGuid(),
                UserName = "testName",
                Content = "testContent",
                DateOfCreation = DateTime.Now
            };

            IEnumerable<PostDto> postDtos = new List<PostDto>() { post };

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Posts.GetPostsByUser(It.IsAny<Guid>()))
                .ReturnsAsync(postDtos);

            var response = helper.Controller.GetPostByUser(Guid.NewGuid());
            //Assert.True((response.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.True((response.Result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }
        #endregion


        #region PostPost
        [Fact]
        public void PostPlant_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PostPost(null));
        }
        #endregion


        #region PutPost
        [Fact]
        public void PutPost_ShouldThrowArgumentNullException_1()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PutPost(null, Guid.NewGuid()));
        }
        #endregion


        #region PutPost
        [Fact]
        public void PutPost_ShouldThrowArgumentNullException_2()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PutPost(new Post(), Guid.Empty));
        }
        #endregion


        #region DeletePost
        [Fact]
        public void DeletePost_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.DeletePost(Guid.Empty));
        }
        #endregion

        private class TestHelper
        {
            protected internal Mock<IPostRepository> mockPlantsRepository;
            protected internal PostsController Controller;
            protected internal Mock<IUnitOfWork> mockUnitOfWork;

            public TestHelper()
            {
                mockPlantsRepository = new Mock<IPostRepository>();
                mockUnitOfWork = new Mock<IUnitOfWork>();
                Controller = new(mockUnitOfWork.Object);
            }
        }
    }
}
