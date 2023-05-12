using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Services;

namespace PlantsAPI.Repositories
{
    public class ReplyRepository : GenericRepository<Reply>, IReplyRepository
    {
        private readonly INotificationService notificationService;
        public ReplyRepository(PlantsDbContext dbContext, IUserContext userContext, INotificationService notificationService) : base(dbContext, userContext)
        {
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        public async Task<IEnumerable<Reply>> GetReplies()
        {
            return await dbSet.ToListAsync();
        }

        public Task<Reply> GetReplyById(Guid replyId)
        {
            if (replyId == Guid.Empty) throw new ArgumentNullException(nameof(replyId));

            var reply = dbSet.Where(r => r.Id == replyId).FirstAsync();
            return reply;
        }

        public async Task<IEnumerable<Reply>> GetRepliesOfPost(Guid postId)
        {
            if (postId == Guid.Empty) throw new ArgumentNullException(nameof(postId));

            
            var post = _dbContext.Posts.Where(x => x.Id == postId).FirstOrDefault();

            if (post != null && _userContext.HasAuthorization(post.UserId))
            {
                List<Reply> replies = await dbSet.Where(r => r.PostId == postId).ToListAsync();

                var repliesInOrder = replies.OrderBy(x => x.DateOfCreation);
                return repliesInOrder;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }

        }

        public async Task<int> GetRepliesCount(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            if (_userContext.HasAuthorization(userId))
            {
                List<Reply> result = await dbSet.Where(p => p.UserId == userId).ToListAsync();
                return result.Count;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
           

        }

        public async Task<Reply> AddReply(Reply reply)
        {
            if (reply == null) throw new ArgumentNullException(nameof(reply));

            reply.Id = Guid.NewGuid();
            var added = await dbSet.AddAsync(reply);
            
             
            if (added != null)
            {
                Post post = _dbContext.Posts.Where(p => p.Id == reply.PostId).First();
                EmailData emailData = new()
                {

                    Recipicent = "ryann.rempel@ethereal.email",
                    Subject = "New Reply",
                    DataName = post.Title,
                    Url = "http://localhost:4200/post/" + post.Id,
                    Body = "2dl víz"
                };

                notificationService.SendEmail(emailData, EmailTemplate.NEWREPLY);

            }
            var result = await GetReplyById(reply.Id);
            return added?.Entity ;
        }

        /*
        public async Task<Reply> EditReply(Reply reply)
        {
            if (reply == null) throw new ArgumentNullException(nameof(reply));

            // dbSet.Update() ??
            var originalReply = await dbSet.Where(r => r.Id == reply.Id).FirstAsync();
            if (originalReply != null)
            {
                originalReply.Content = reply.Content;
                originalReply.DateOfCreation = reply.DateOfCreation;
            }
            return originalReply;
        }*/

        public async Task<bool> DeleteReply(Guid replyId)
        {
            if (replyId == Guid.Empty) throw new ArgumentNullException(nameof(replyId));

            var toBeDeleted = await dbSet.Where(r => r.Id == replyId).FirstAsync();

            var result = dbSet.Remove(toBeDeleted);

            return result.State == EntityState.Deleted;
        }
    }
}
