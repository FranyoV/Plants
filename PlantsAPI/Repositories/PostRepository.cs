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

        //anonymousaccess
        public async Task<IEnumerable<PostDto>> GetPosts()
        {
            List<Post> posts = new();
            List<PostDto> postDtos = new();
            posts = await dbSet.ToListAsync();

            if (posts.Count > 0)
            {
                foreach (var post in posts)
                {
                    User user = await _dbContext.Users.Where(x => x.Id == post.UserId).FirstAsync();
                    int replyCount = await _dbContext.Replies.Where(x => x.PostId == post.Id).CountAsync();

                    PostDto postDto = new()
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Content = post.Content,
                        DateOfCreation = post.DateOfCreation,
                        ImageUrl = post.ImageUrl,
                        UserName = user.Name,
                        UserId = post.UserId,
                        ReplyCount = replyCount,
                    };
                    postDtos.Add(postDto);
                }
                
            }
            var postsInOrder = postDtos.OrderByDescending(x => x.DateOfCreation);

            return postsInOrder;
        }

        //anonymous allowed
        public async Task<PostDto> GetPostById(Guid postId)
        {
            if (postId == Guid.Empty) throw new ArgumentNullException(nameof(postId));

            Post post = new();
            PostDto dto = new();
            post = await dbSet.Where(p => p.Id == postId).FirstAsync();
            User user = await _dbContext.Users.Where(u => u.Id == post.UserId).FirstAsync();

            dto = new(){
                Id = post.Id,
                UserId = post.UserId,
                Title = post.Title,
                Content = post.Content,
                DateOfCreation = post.DateOfCreation,
                ImageUrl = post.ImageUrl,
                UserName = user.Name,
                ReplyCount = 0
            };

            return dto;

        }

        //üres lista
        public async Task<IEnumerable<PostDto>> GetPostsOfUser(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            if (_userContext.HasAuthorization(id))
            {
                List<Post> postsOfUser = await dbSet.Where(p => p.UserId == id).ToListAsync();
                List<PostDto> postDtos = new();


                if (postsOfUser.Count > 0)
                {
                    foreach (var post in postsOfUser)
                    {
                        var user = _dbContext.Users.FirstOrDefault(x => x.Id == post.UserId);
                        var count = _dbContext.Replies.Where(r => r.PostId == post.Id).Count();
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
                                UserId = post.UserId,
                                ReplyCount = count
                            };
                            postDtos.Add(postDto);
                        }
                    }
                }
                var postsInOrder = postDtos.OrderByDescending(x => x.DateOfCreation);

                return postsInOrder;
            }
            return new List<PostDto>();

        }

        //üres lista
        public async Task<IEnumerable<PostDto>> GetPostsByUserReplies(Guid userId)
        {
            if ( userId == Guid.Empty ) throw new ArgumentNullException(nameof(userId));

            if ( _userContext.HasAuthorization(userId) )
            {
                List<Reply> repliesOfUser = await _dbContext.Replies.Where(r => r.UserId == userId).ToListAsync();
                var repliesOfUserOrdered = repliesOfUser.OrderByDescending(x => x.DateOfCreation);

                List<Post> postsWithUsersReplies = new();
                List<PostDto> postDtos = new();


                foreach (var reply in repliesOfUserOrdered)
                {
                    var posts = await _dbContext.Posts.Where(p => p.Id == reply.PostId).ToListAsync();

                    if (posts != null)
                    {
                        foreach (var item in posts)
                        {
                            if (!postsWithUsersReplies.Contains(item))
                            {
                                postsWithUsersReplies.AddRange(posts);
                            }
                        }

                    }
                }


                if (postsWithUsersReplies.Count > 0)
                {
                    foreach (var post in postsWithUsersReplies)
                    {
                        var user = _dbContext.Users.FirstOrDefault(x => x.Id == post.UserId);
                        var count = _dbContext.Replies.Where(r => r.PostId == post.Id).Count();

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
                                UserId = post.UserId,
                                ReplyCount = count
                            };
                            postDtos.Add(postDto);

                        }
                    }
                }
                //var postsInOrder = postDtos.OrderByDescending(x => x.DateOfCreation);


                return postDtos;
            }
            return new List<PostDto>();
        }


        public async Task<int> GetPostsCount(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            List<Post> result = new();

            if (_userContext.HasAuthorization(userId))
            {
                result = await dbSet.Where(p => p.UserId == userId).ToListAsync();
                return result.Count;
            }
            return result.Count ;

        }


        //throw exception
        public async Task<Post> AddPost(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));

            if (_userContext.HasAuthorization(post.UserId))
            {
                var result = await dbSet.AddAsync(post);
                return result.Entity;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }


        //NOT USED
        
        //public async Task<Post> EditPost(Post post)
        //{
        //    if (post == null) throw new ArgumentNullException(nameof(post));

        //    if ( _userContext.HasAuthorization(post.UserId) )
        //    {
        //        var originalPost = await dbSet.FirstAsync(p => p.Id == post.Id);

        //        if (originalPost != null)
        //        {
        //            originalPost.Title = post.Title;
        //            originalPost.Content = post.Content;
        //            originalPost.DateOfCreation = post.DateOfCreation;
        //        }

        //        return originalPost;
        //    }
        //    else
        //    {
        //        throw new UnauthorizedAccessException();
        //    }
        //}

        //return false
        public async Task<bool> DeletePost(Guid postId)
        {
            if (postId == Guid.Empty) throw new ArgumentNullException(nameof(postId));

            Guid currentUserId = Guid.Parse(_userContext.GetMe()); ;
            
            var isPostUsers = dbSet.Where(p => p.Id == postId).Any(up => up.UserId == currentUserId);

            if (isPostUsers)
            {
                var toBeDeleted = await dbSet.Where(p => p.Id == postId).FirstAsync();
                if (toBeDeleted != null)
                {
                    var result = dbSet.Remove(toBeDeleted);
                    return result.State == EntityState.Deleted;
                }
            }
            return false;

        }


    }
}
