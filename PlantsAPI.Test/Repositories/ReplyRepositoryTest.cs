using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Repositories;
using PlantsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PlantsAPI.Test.Repositories
{
    public class ReplyRepositoryTest
    {
        private readonly PlantsDbContext context;
        //private readonly Mock<ILogger> logger;
        private readonly IReplyRepository replyRepository;
        private readonly Mock<IConfiguration> configuration;
        private readonly Mock<INotificationService> notificationService;
        private readonly Mock<IUserContext> userContext;

        public ReplyRepositoryTest()
        {
            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            configuration = new Mock<IConfiguration>();
            context = new PlantsDbContext(dbOptions.Options);
            //logger = new Mock<ILogger>();
            notificationService = new Mock<INotificationService>();
            replyRepository = new ReplyRepository(context, userContext.Object, notificationService.Object);
        }

        #region Constructor
        [Fact]
        public void ContructorShouldCreateObject()
        {
            Assert.NotNull(new ReplyRepository(context, userContext.Object, notificationService.Object));
        }

        [Fact]
        public void Contructor_ShoulThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new ReplyRepository(context, userContext.Object, notificationService.Object));
        }

        [Fact]
        public void Contructor_ShoulThrowArgumentNullException_2()
        {
            Assert.Throws<ArgumentNullException>(() => new ReplyRepository(null, userContext.Object, notificationService.Object));
        }

        [Fact]
        public void Contructor_ShoulThrowArgumentNullException_3()
        {
            Assert.Throws<ArgumentNullException>(() => new ReplyRepository(context, userContext.Object, null));
        }
        #endregion

       /* #region GetReplies
        [Fact]
        public void GetReplies_ShouldReturnResult()
        {
            var itemList = replyRepository.GetReplies();
            Assert.NotNull(itemList.Result);
            Assert.NotEmpty(itemList.Result);
        }
        #endregion*/


        #region GetRepliesOfPost
        [Fact]
        public void GetRepliesOfPost_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => replyRepository.GetRepliesOfPost(Guid.Empty));
        }
        #endregion


        #region GetRepliesCount

        [Fact]
        public void GetRepliesCount_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => replyRepository.GetRepliesCount(Guid.Empty));
        }

        #endregion


        #region AddReply

        [Fact]
        public void AddReply_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => replyRepository.AddReply(null));
        }

        #endregion

        /*
        #region EditReply

        [Fact]
        public void EditReply_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => replyRepository.EditReply(null));
        }

        #endregion

        #region DeleteReply

        [Fact]
        public void DeleteReply_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => replyRepository.DeleteReply(Guid.Empty));
        }

        #endregion
        */
    }
}
