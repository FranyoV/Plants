using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsAPI.Data;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(PlantsDbContext dbContext, ILogger logger) : base(dbContext, logger)
        {

        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<Post> GetPostById(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            var post = await dbSet.Where(p => p.Id == id).FirstAsync();

            return post;
        }

        public async Task<Post> AddPost(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));

            var result = dbSet.Add(post);
            return result.Entity;
        }

        public async Task<Post> EditPost(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));

            var originalPost = await dbSet.FirstAsync(p => p.Id == post.Id);

            if (originalPost != null)
            {
                originalPost.Title = post.Title;
                originalPost.Content = post.Content;
                originalPost.DateOfCreation = post.DateOfCreation;
            }

            return originalPost;
        }


        public async Task<bool> DeletePost(Guid postId)
        {
            if (postId == Guid.Empty) throw new ArgumentNullException(nameof(postId));

            var toBeDeleted = await dbSet.Where(p => p.Id == postId).FirstAsync();
            if (toBeDeleted != null)
            {
                var result = dbSet.Remove(toBeDeleted);
                return result.State == EntityState.Deleted;
            }

            return false;
        }


    }
}
