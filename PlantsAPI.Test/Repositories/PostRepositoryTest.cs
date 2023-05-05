using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PlantsAPI.Data;
using PlantsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PlantsAPI.Test.Repositories
{
    public class PostRepositoryTest
    {
        private readonly PlantsDbContext context;
        private readonly Mock<ILogger> logger;
        private readonly IPostRepository postRepository;

        public PostRepositoryTest()
        {

            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            context = new PlantsDbContext(dbOptions.Options);
            logger = new Mock<ILogger>();
            postRepository = new PostRepository(context, logger.Object);
        }

        #region Constructor
        [Fact]
        public void ContructorShouldCreateObject()
        {
            Assert.NotNull(new PostRepository(context, logger.Object));
        }

        [Fact]
        public void Contsructor_ShouldThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new PostRepository(null, logger.Object));
        }

        [Fact]
        public void Contsructor_ShouldThrowArgumentNullException_2()
        {
            Assert.Throws<ArgumentNullException>(() => new PostRepository(context, null));
        }
        #endregion

        #region GetPosts
        [Fact]
        public void GetPosts_ShouldReturnResult()
        {
            var itemList = postRepository.GetPosts();
            Assert.NotNull(itemList.Result);
            Assert.NotEmpty(itemList.Result);
        }
        #endregion


        #region GetPostById
        public void GetPostById_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.GetPostById(Guid.Empty));
        }
        #endregion


        #region GetPostsByOfUser

        [Fact]
        public void GetPostsByOfUser_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.GetPostsOfUser(Guid.Empty));
        }

        #endregion


        #region GetPostsByUserReplies
        [Fact]
        public void GetPostsByUserReplies_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.GetPostsByUserReplies(Guid.Empty));
        }
        #endregion


        #region GetPostsCount
        [Fact]
        public void GetPostsCount_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.GetPostsCount(Guid.Empty));
        }
        #endregion


        #region AddPost

        [Fact]
        public void AddPost_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.AddPost(null));
        }

        #endregion

        #region EditPost

        [Fact]
        public void EditItem_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.EditPost(null));
        }

        #endregion

        #region  DeletePost

        [Fact]
        public void DeleteItem_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => postRepository.DeletePost(Guid.Empty));
        }

        #endregion
    }
}
