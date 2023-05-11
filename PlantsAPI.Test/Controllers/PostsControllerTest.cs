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


        #region GetPosts


        [Fact]
        public void GetPost_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.Posts.GetPosts())
                .ReturnsAsync(It.IsAny<List<PostDto>>);

            var response = helper.Controller.GetPosts();

            Assert.True((response.Result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }
        #endregion


        #region GetPostById

        [Fact]
        public void GetPostById_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.GetPostById(Guid.Empty));
        }


        [Fact]
        public void GetPostById_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.Posts.GetPostById(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<Post>);

            var response = helper.Controller.GetPostById(Guid.NewGuid());

            Assert.True((response.Result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.OK);
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
        public void GetPostByUser_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Posts.GetPostsOfUser(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<List<PostDto>>);

            var response = helper.Controller.GetPostByUser(Guid.NewGuid());
            
            Assert.True((response.Result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }


        [Fact]
        public void GetPostByUser_ShouldReturnUnauthorized()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var response = helper.Controller.GetPostByUser(Guid.NewGuid());
            
            Assert.True((response?.Result.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized);
        }

        #endregion

        #region GetPostByUserReplies

        [Fact]
        public void GetPostByUserReplies_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.GetPostByUserReplies(Guid.Empty));
        }


        [Fact]
        public void GetPostByUserReplies_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Posts.GetPostsByUserReplies(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<List<PostDto>>);

            var response = helper.Controller.GetPostByUserReplies(Guid.NewGuid());

            Assert.True((response.Result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }


        [Fact]
        public void GetPostByUserReplies_ShouldReturnUnauthorized()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var response = helper.Controller.GetPostByUserReplies(Guid.NewGuid());

            Assert.True((response?.Result.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized);
        }

        #endregion


        #region GetPostsCount
        [Fact]
        public void GetPostsCount_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.GetPostsCount(Guid.Empty));
        }


        [Fact]
        public void GetPostsCount_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Posts.GetPostsCount(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<int>);

            var response = helper.Controller.GetPostsCount(Guid.NewGuid());

            Assert.True((response.Result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }


        [Fact]
        public void GetPostsCount_ShouldReturnUnauthorized()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var response = helper.Controller.GetPostsCount(Guid.NewGuid());

            Assert.True((response?.Result.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized);
        }

        #endregion



        #region PostPost
        [Fact]
        public void PostPlant_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PostPost(null));
        }


        [Fact]
        public void PostPost_ShouldReturnOk()
        {
            TestHelper helper = new();
           
            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Posts.AddPost(It.IsAny<Post>()))
                .ReturnsAsync(It.IsAny<Post>);

            var response = helper.Controller.PostPost(new Post());
            
            Assert.True((response.Result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }

        [Fact]
        public void PostPost_ShouldReturnUnauthorized()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var response = helper.Controller.PostPost(new Post());

            Assert.True((response.Result.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized);
        }
        #endregion


        #region PutPost
        [Fact]
        public void PutPost_ShouldThrowArgumentNullException_1()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PutPost(null, Guid.NewGuid()));
        }

        [Fact]
        public void PutPost_ShouldThrowArgumentNullException_2()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PutPost(new Post(), Guid.Empty));
        }


        [Fact]
        public void PutPost_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Posts.EditPost(It.IsAny<Post>()))
                .ReturnsAsync(It.IsAny<Post>);

            var response = helper.Controller.PutPost( new Post(), Guid.NewGuid());

            Assert.True((response.Result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }


        [Fact]
        public void PutPost_ShouldReturnUnauthorized()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var response = helper.Controller.PutPost(new Post(), Guid.NewGuid());

            Assert.True((response.Result.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized);
        }
        #endregion


        #region DeletePost
        [Fact]
        public void DeletePost_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.DeletePost(Guid.Empty));
        }

        [Fact]
        public void DeletePost_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Posts.DeletePost(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<bool>);

            var response = helper.Controller.DeletePost( Guid.NewGuid());

            Assert.True((response.Result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }

        [Fact]
        public void DeletePost_ShouldReturnUnauthorized()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var response = helper.Controller.DeletePost(Guid.NewGuid());

            Assert.True((response.Result.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized);
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
