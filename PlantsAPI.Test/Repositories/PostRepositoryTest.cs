using Microsoft.EntityFrameworkCore;
using Moq;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Repositories;
using PlantsAPI.Services;
using System;
using System.Linq;
using Xunit;

namespace PlantsAPI.Test.Repositories
{

    public class PostRepositoryTest
    {
        private readonly PlantsDbContext dbContext;
        private readonly Mock<IUserContext> userContext;
        private readonly IPostRepository postRepository;

        public PostRepositoryTest()
        {
            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            dbContext = new PlantsDbContext(dbOptions.Options);

            DbInitializer.Initialize(dbContext);

            userContext = new Mock<IUserContext>();
            postRepository = new PostRepository(dbContext, userContext.Object);
        }


        #region Constructor
        [Fact]
        public void ContructorShouldCreateObject()
        {
            Assert.NotNull(new PostRepository(dbContext, userContext.Object));
        }

        [Fact]
        public void Contsructor_ShouldThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new PostRepository(null, userContext.Object));
        }

        [Fact]
        public void Contsructor_ShouldThrowArgumentNullException_2()
        {
            Assert.Throws<ArgumentNullException>(() => new PostRepository(dbContext, null));
        }
        #endregion

        #region GetPosts
        [Fact]
        public void GetPosts_ShouldReturnResult()
        {
            PostDto testPostDto = new()
            {
                Id = Guid.Parse("c1a1e629-7d42-4b6a-8fa1-6508f9c594ec"),
                Title = "My cactus is dying.",
                Content = "Why can't I keep a cactus alive. They supposed to stay alive even in deserts.",
                DateOfCreation = DateTime.Now,
                ImageData = null,
                UserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab"),
                UserName = "Virág98",

            };
            var postList = postRepository.GetPosts();

            Assert.NotNull(postList.Result);
            Assert.NotEmpty(postList.Result);
            Assert.Single(postList.Result);
            Assert.Equal(testPostDto.Id, postList.Result.First().Id);

            Assert.Equal(testPostDto.Title, postList.Result.First().Title);
            Assert.Equal(testPostDto.Content, postList.Result.First().Content);
            Assert.Equal(testPostDto.UserName, postList.Result.First().UserName);
            Assert.Equal(testPostDto.UserId, postList.Result.First().UserId);

        }
        #endregion


        #region GetPostById
        public void GetPostById_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.GetPostById(Guid.Empty));
        }


        [Fact]
        public void GetPostById_ShouldReturnResult()
        {
            PostDto testPostDto = new()
            {
                Id = Guid.Parse("c1a1e629-7d42-4b6a-8fa1-6508f9c594ec"),
                Title = "My cactus is dying.",
                Content = "Why can't I keep a cactus alive. They supposed to stay alive even in deserts.",
                DateOfCreation = DateTime.Now,
                ImageData = null,
                UserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab"),
                UserName = "Virág98",

            };
            var postDtoResult = postRepository.GetPostById(Guid.Parse("c1a1e629-7d42-4b6a-8fa1-6508f9c594ec"));

            Assert.NotNull(postDtoResult.Result);
          
            Assert.Equal(testPostDto.Id, postDtoResult.Result.Id);

            Assert.Equal(testPostDto.Title, postDtoResult.Result.Title);
            Assert.Equal(testPostDto.Content, postDtoResult.Result.Content);
            Assert.Equal(testPostDto.UserName, postDtoResult.Result.UserName);
            Assert.Equal(testPostDto.UserId, postDtoResult.Result.UserId);

        }

        #endregion


        #region GetPostsOfUser

        [Fact]
        public void GetPostsOfUser_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.GetPostsOfUser(Guid.Empty));
        }

        [Fact]
        public void GetPostsOfUser_ShouldReturnResult()
        {
            PostDto testPostDto = new()
            {
                Id = Guid.Parse("c1a1e629-7d42-4b6a-8fa1-6508f9c594ec"),
                Title = "My cactus is dying.",
                Content = "Why can't I keep a cactus alive. They supposed to stay alive even in deserts.",
                DateOfCreation = DateTime.Now,
                ImageData = null,
                UserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab"),
                UserName = "Virág98",

            };

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>())).
                Returns(true);

            var postList = postRepository.GetPostsOfUser(Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab"));
           
            Assert.NotNull(postList.Result);
            Assert.NotEmpty(postList.Result);
            Assert.Single(postList.Result);

            Assert.Equal(testPostDto.Id, postList.Result.First().Id);
            Assert.Equal(testPostDto.Title, postList.Result.First().Title);
            Assert.Equal(testPostDto.Content, postList.Result.First().Content);
            Assert.Equal(testPostDto.UserName, postList.Result.First().UserName);
            Assert.Equal(testPostDto.UserId, postList.Result.First().UserId);

        }


        [Fact]
        public void GetPostsOfUser_ShouldReturnEmptyResult()
        {
            Guid existingUserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab");

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>())).
                Returns(false);

            var postList = postRepository.GetPostsOfUser(existingUserId);

            Assert.NotNull(postList.Result);
            Assert.Empty(postList.Result);
           
        }



        #endregion


        #region GetPostsByUserReplies
        [Fact]
        public void GetPostsByUserReplies_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.GetPostsByUserReplies(Guid.Empty));
        }



        [Fact]
        public void GetPostsByUserReplies_ShouldReturnResult()
        {
            PostDto testPostDto = new()
            {
                Id = Guid.Parse("c1a1e629-7d42-4b6a-8fa1-6508f9c594ec"),
                Title = "My cactus is dying.",
                Content = "Why can't I keep a cactus alive. They supposed to stay alive even in deserts.",
                DateOfCreation = DateTime.Now,
                ImageData = null,
                UserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab"),
                UserName = "Virág98",

            };

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>())).
                Returns(true);

            var postList = postRepository.GetPostsByUserReplies(testPostDto.UserId);

            Assert.NotNull(postList.Result);
            Assert.NotEmpty(postList.Result);
            Assert.Single(postList.Result);

            Assert.Equal(testPostDto.Id, postList.Result.First().Id);
            Assert.Equal(testPostDto.Title, postList.Result.First().Title);
            Assert.Equal(testPostDto.Content, postList.Result.First().Content);
            Assert.Equal(testPostDto.UserName, postList.Result.First().UserName);
            Assert.Equal(testPostDto.UserId, postList.Result.First().UserId);

        }


        [Fact]
        public void GetPostsByUserReplies_ShouldReturnEmptyResult()
        {
            Guid existingUserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab");

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>())).
                Returns(false);

            var postList = postRepository.GetPostsByUserReplies(existingUserId);

            Assert.NotNull(postList.Result);
            Assert.Empty(postList.Result);

        }
        #endregion


        #region GetPostsCount
        [Fact]
        public void GetPostsCount_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.GetPostsCount(Guid.Empty));
        }


        [Fact]
        public void GetPostsCount_ShouldReturnResult()
        {
            Guid existingUserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab");

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>())).
                Returns(true);

            var postCount = postRepository.GetPostsCount(existingUserId);

            Assert.NotNull(postCount.Result);
            Assert.Equal(1, postCount.Result);

        }


        [Fact]
        public void GetPostsCount_ShouldReturn0()
        {
            Guid existingUserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab");

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>())).
                Returns(false);

            var postCount = postRepository.GetPostsCount(existingUserId);

            Assert.NotNull(postCount.Result);
            Assert.Equal(0, postCount.Result);

        }
        #endregion


        #region AddPost

        [Fact]
        public void AddPost_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.AddPost(null));
        }


        [Fact]
        public void AddPostt_ShouldReturnResult()
        {
            Guid existingUserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab");

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>())).
                Returns(true);

            var postCount = postRepository.AddPost(new Post());

            Assert.NotNull(postCount.Result);
           // Assert.Equal(1, postCount.Result);

        }



        [Fact]
        public void AddPost_ShouldReturnUnauthrorizedException()
        {
            Guid existingUserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab");

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>())).
                Returns(false);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => postRepository.AddPost(new Post()));

        }
        #endregion

        //#region EditPost

        //[Fact]
        //public void EditItem_ShouldThrowArgumentNullException()
        //{
        //    Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.EditPost(null));
        //}

        //#endregion

        #region  DeletePost

        [Fact]
        public void DeleteItem_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.DeletePost(Guid.Empty));
        }

        [Fact]
        public void DeletePost_ShouldReturnFalse()
        {
            Guid existingPostId = Guid.Parse("c1a1e629-7d42-4b6a-8fa1-6508f9c594ec");

            userContext.Setup(
                x => x.GetMe()).
                Returns("7a02b8d4-570d-404f-946e-40b4524727ab");

            Assert.True(postRepository.DeletePost(existingPostId).Result);

        }


        [Fact]
        public void AddPost_ShouldReturnFalse()
        {
            Guid existingPostId = Guid.Parse("c1a1e629-7d42-4b6a-8fa1-6508f9c594ec");

            userContext.Setup(
                x => x.GetMe()).
                Returns("2926d291-b549-429c-b4a7-6cdfbbca322e");

            Assert.False(postRepository.DeletePost(existingPostId).Result);

        }
        #endregion
    }
}
