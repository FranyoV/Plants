using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public interface IReplyRepository
    {
        // Task<IEnumerable<Reply>> GetReplies();
        Task<Reply> GetReplyById(Guid replyId);
        Task<IEnumerable<ReplyDto>> GetRepliesOfPost(Guid postId);
        Task<int> GetRepliesCount(Guid userId);
        Task<ReplyDto> AddReply(Reply reply);
        // Task<Reply> EditReply(Reply reply);
        Task<bool> DeleteReply(Guid replyId);
    }
}
