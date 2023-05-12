using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Services;

namespace PlantsAPI.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(PlantsDbContext dbContext, IUserContext userContext) : base(dbContext, userContext)
        {
        }

        public async Task<IEnumerable<PostDto>> GetPosts()
        {
            List<Post> posts = new();
            List<PostDto> postDtos = new();
            posts = await dbSet.ToListAsync();

            if (posts.Count > 0)
            {
                foreach (var post in posts)
                {
                    var user = _dbContext.Users.FirstOrDefault(x => x.Id == post.UserId);

                    PostDto postDto = new()
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Content = post.Content,
                        DateOfCreation = post.DateOfCreation,
                        ImageUrl = post.ImageUrl,
                        UserName = user.Name,
                        UserId = post.UserId
                    };
                    postDtos.Add(postDto);
                }
                
            }
            var postsInOrder = postDtos.OrderByDescending(x => x.DateOfCreation);

            return postsInOrder;
        }


        public async Task<Post> GetPostById(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            var post = await dbSet.Where(p => p.Id == id).FirstAsync();

            return post;
        }


        public async Task<IEnumerable<PostDto>> GetPostsOfUser(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            List<Post> postsOfUser = await dbSet.Where(p => p.UserId == id).ToListAsync();
            List<PostDto> postDtos = new();
            

            if (postsOfUser.Count > 0)
            {
                foreach (var post in postsOfUser)
                {
                    var user = _dbContext.Users.FirstOrDefault(x => x.Id == post.UserId);

                    if (user != null)
                    {
                        PostDto postDto = new()
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Content = post.Content,
                            DateOfCreation = post.DateOfCreation,
                            ImageUrl = post.ImageUrl,
                            UserName = user.Name,
                            UserId = post.UserId
                        };
                        postDtos.Add(postDto);
                    }
                }
            }
            var postsInOrder = postDtos.OrderByDescending(x => x.DateOfCreation);

            return postsInOrder;
        }


        public async Task<IEnumerable<PostDto>> GetPostsByUserReplies(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            List<Reply> repliesOfUser = await _dbContext.Replies.Where(r => r.UserId == userId).ToListAsync();
            var repliesOfUserOrdered = repliesOfUser.OrderByDescending(x => x.DateOfCreation);

            List<Post> postsWithUsersReplies = new();
            List<PostDto> postDtos = new();


            foreach (var reply in repliesOfUserOrdered)
            {
                var post = await _dbContext.Posts.Where(p => p.Id == reply.PostId).ToListAsync();
                if (post != null)
                {
                    postsWithUsersReplies.AddRange(post);
                } 
            }
            

            if (postsWithUsersReplies.Count > 0)
            {
                foreach (var post in postsWithUsersReplies)
                {
                    var user = _dbContext.Users.FirstOrDefault(x => x.Id == post.UserId);
                    if (user != null)
                    {
                        PostDto postDto = new()
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Content = post.Content,
                            DateOfCreation = post.DateOfCreation,
                            ImageUrl = post.ImageUrl,
                            UserName = user.Name,
                            UserId = post.UserId
                        };
                        postDtos.Add(postDto);

                    }
                }

            }
            //var postsInOrder = postDtos.OrderByDescending(x => x.DateOfCreation);


            return postDtos;
        }


        public async Task<int> GetPostsCount(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            List<Post> result = await dbSet.Where(p => p.UserId == userId).ToListAsync();
            return result.Count;

        }


        public async Task<Post> AddPost(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));

            var result = await dbSet.AddAsync(post);
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
