using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Repositories;
using PlantsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PlantsAPI.Test.Repositories
{
    public class ReplyRepositoryTest
    {
        private readonly PlantsDbContext dbContext;
        private readonly Mock<IUserContext> userContext;
        private readonly IReplyRepository replyRepository;
        private readonly Mock<IConfiguration> configuration;
        private readonly Mock<INotificationService> notificationService;
        public ReplyRepositoryTest()
        {
            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            configuration = new Mock<IConfiguration>();
            dbContext = new PlantsDbContext(dbOptions.Options);

            DbInitializer.Initialize(dbContext);

            userContext = new Mock<IUserContext>();
            notificationService = new Mock<INotificationService>();
            replyRepository = new ReplyRepository(dbContext, userContext.Object, notificationService.Object);
        }



        #region Constructor
        [Fact]
        public void ConstructorShouldCreateObject()
        {
            Assert.NotNull(new ReplyRepository(dbContext, userContext.Object, notificationService.Object));
        }


        [Fact]
        public void Constructor_ShoulThrowArgumentNullException_2()
        {
            Assert.Throws<ArgumentNullException>(() => new ReplyRepository(null, userContext.Object, notificationService.Object));
        }

        [Fact]
        public void Constructor_ShoulThrowArgumentNullException_3()
        {
            Assert.Throws<ArgumentNullException>(() => new ReplyRepository(dbContext, userContext.Object, null));
        }
        #endregion


        #region GetRepliesById

        [Fact]
        public void GetRepliesById_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => replyRepository.GetRepliesCount(Guid.Empty));
        }


        [Fact]
        public void GetRepliesById_ShouldReturnResult()
        {
            Reply testReply = new Reply
            {
                Id = Guid.Parse("e41ca1f8-7fda-4610-bcce-f63972b6e302"),
                Content = "Dudee water it once every two weeks and put the bad boy by the window and you both should be good.",
                DateOfCreation = DateTime.Now,
                PostId = Guid.Parse("c1a1e629-7d42-4b6a-8fa1-6508f9c594ec"),
                UserId = Guid.Parse("20526112-56fa-4e4a-8ae5-1d3603aa47e9")
            };

            var result = replyRepository.GetReplyById(testReply.Id);


            Assert.NotNull(result.Result);
            Assert.Equal(testReply.Id, result.Result.Id);
            Assert.Equal(testReply.Content, result.Result.Content);
            Assert.Equal(testReply.UserId, result.Result.UserId);
        }

        #endregion


        #region GetRepliesOfPost

        [Fact]
        public void GetRepliesOfPost_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => replyRepository.GetRepliesOfPost(Guid.Empty));
        }

        [Fact]
        public void GetRepliesOfPost_ShouldReturnResult()
        {
            Reply testReply = new Reply
            {
                Id = Guid.Parse("e41ca1f8-7fda-4610-bcce-f63972b6e302"),
                Content = "Dudee water it once every two weeks and put the bad boy by the window and you both should be good.",
                DateOfCreation = DateTime.Now,
                PostId = Guid.Parse("c1a1e629-7d42-4b6a-8fa1-6508f9c594ec"),
                UserId = Guid.Parse("20526112-56fa-4e4a-8ae5-1d3603aa47e9")
            };
            
            var repliesOfPost = replyRepository.GetRepliesOfPost(testReply.PostId);


            Assert.NotNull(repliesOfPost.Result);
            Assert.NotEmpty(repliesOfPost.Result);
            Assert.Equal(testReply.Id, repliesOfPost.Result.First().Id);
            Assert.Equal(testReply.Content, repliesOfPost.Result.First().Content);
            Assert.Equal(testReply.UserId, repliesOfPost.Result.First().UserId);
            // Assert.Equal(//0, result.Result);
        }

        #endregion



        #region GetRepliesCount
        [Fact]
        public void GetRepliesCount_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => replyRepository.GetRepliesCount(Guid.Empty));
        }


        [Fact]
        public void GetRepliesCount_ShouldReturnEmptyList()
        {
            Guid existingUserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab");
            var result = replyRepository.GetRepliesCount(existingUserId);
            Assert.NotNull(result.Result);
            Assert.Equal(1, result.Result);
        }

        #endregion

        #region AddReply

        [Fact]
        public void AddReply_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => replyRepository.AddReply(null));
        }



        [Fact]
        public void AddReply_ShouldHaveNoAuthorization()
        {

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => replyRepository.AddReply(new Reply()));
        }



        [Fact]
        public void AddReply_ShouldReturnResult()
        {
            Reply testReply = new Reply
            {
                Id = Guid.Parse("3f898e1b-5845-4907-8f20-fc248d6b4b10"),
                Content = "testContent",
                UserId = Guid.Parse("20526112-56fa-4e4a-8ae5-1d3603aa47e9")

            };

            userContext.Setup(
                 x => x.HasAuthorization(It.IsAny<Guid>()))
                 .Returns(true);


            var result = replyRepository.AddReply(testReply);

            Assert.NotNull(result);

            Assert.True(testReply.Id == result.Result.Id);
        }

        #endregion

     
        #region DeletePlant

        [Fact]
        public void DeletePlant_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => replyRepository.DeleteReply(Guid.Empty));
        }


        [Fact]
        public void DeleteReply_ShouldHaveNoAuthorization()
        {
            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => replyRepository.DeleteReply(Guid.NewGuid()));
        }



        [Fact]
        public void DeleteReply_ShouldReturnTrue()
        {
            Reply testReply = new()
            {
                Id = Guid.Parse("e41ca1f8-7fda-4610-bcce-f63972b6e302"),
                Content = "Dudee water it once every two weeks and put the bad boy by the window and you both should be good.",
                DateOfCreation = DateTime.Now,
                PostId = Guid.Parse("c1a1e629-7d42-4b6a-8fa1-6508f9c594ec"),
                UserId = Guid.Parse("20526112-56fa-4e4a-8ae5-1d3603aa47e9")
            };

            userContext.Setup(
                 x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            var result = replyRepository.DeleteReply(testReply.Id);

            Assert.NotNull(result);
            Assert.True(result.Result);
        }

        [Fact]
        public void DeleteReply_ShouldReturnFalse()
        {
            Reply testReply = new()
            {
                Id = Guid.Parse("e41ca1f8-7fda-4610-bcce-f63972b6e302"),
                Content = "Dudee water it once every two weeks and put the bad boy by the window and you both should be good.",
                DateOfCreation = DateTime.Now,
                PostId = Guid.Parse("c1a1e629-7d42-4b6a-8fa1-6508f9c594ec"),
                UserId = Guid.Parse("20526112-56fa-4e4a-8ae5-1d3603aa47e9")
            };

            userContext.Setup(
                 x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var result = replyRepository.DeleteReply(testReply.Id);

            Assert.NotNull(result);
            Assert.False(result.Result);
        }
        #endregion


    }
}
