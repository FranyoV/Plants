﻿using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<PostDto>> GetPosts();
        Task<IEnumerable<PostDto>> GetPostsOfUser(Guid id);
        Task<IEnumerable<PostDto>> GetPostsByUserReplies(Guid userid);
        Task<int> GetPostsCount(Guid userId);
        Task<PostDto> GetPostById(Guid id);
        Task<Post> AddPost(Post post);
        //Task<Post> EditPost(Post post);
        Task<bool> DeletePost(Guid postId);
    }
}
