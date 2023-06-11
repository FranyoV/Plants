﻿using Microsoft.AspNetCore.Mvc;
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
                    User user = await _dbContext.Users.Where(x => x.Id == post.UserId).FirstAsync();
                    int replyCount = await _dbContext.Replies.Where(x => x.PostId == post.Id).CountAsync();

                    PostDto postDto = new()
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Content = post.Content,
                        DateOfCreation = post.DateOfCreation,
                        ImageData = post.ImageData,
                        UserName = user.Name,
                        ProfileImage = user.ImageData,
                        UserId = post.UserId,
                        ReplyCount = replyCount,
                    };
                    postDtos.Add(postDto);
                }
                
            }
            var postsInOrder = postDtos.OrderByDescending(x => x.DateOfCreation);

            return postsInOrder;
        }


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
                ImageData = post.ImageData,
                UserName = user.Name,
                ProfileImage = user.ImageData,
                ReplyCount = 0
            };

            return dto;

        }

     
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
                                ImageData = post.ImageData,
                                UserName = user.Name,
                                ProfileImage = user.ImageData,
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
                                ImageData = post.ImageData,
                                UserName = user.Name,
                                ProfileImage = user.ImageData,
                                UserId = post.UserId,
                                ReplyCount = count
                            };
                            postDtos.Add(postDto);

                        }
                    }
                }
               
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



        public async Task<Post> EditPost(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));

            if (_userContext.HasAuthorization(post.UserId))
            {
                var originalPost = await dbSet.FirstAsync(p => p.Id == post.Id);

                if (originalPost != null)
                {
                    originalPost.Title = post.Title;
                    originalPost.Content = post.Content;
                    originalPost.DateOfCreation = post.DateOfCreation;
                    originalPost.ImageData = post.ImageData;
                }

                return originalPost;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        
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



        public async Task<Post> AddImageToPost(Guid id, IFormFile image)
        {
            var planty = image;
            byte[] imageByteArray = null;
            using (var readStream = image.OpenReadStream())
            using (var memoryStream = new MemoryStream())
            {
                readStream.CopyTo(memoryStream);
                imageByteArray = memoryStream.ToArray();
            }
            var todb = imageByteArray;

            Post newPost = new();
           
            if (image != null)
            {
                newPost = await dbSet.Where(p => p.Id == id).FirstAsync();
                newPost.ImageData = todb;
            }
            return newPost;
        }
    }
}
