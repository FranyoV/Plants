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

        /*
        public async Task<IEnumerable<Reply>> GetReplies()
        {
            return await dbSet.ToListAsync();
        }*/

        
        //only used by backend - helper method
        public async Task<Reply> GetReplyById(Guid replyId)
        {
            if (replyId == Guid.Empty) throw new ArgumentNullException(nameof(replyId));
           
            Reply reply = new();
            reply = await dbSet.Where(r => r.Id == replyId).Include(u => u.User.Name).FirstAsync();
            
            return reply;
        }


        //anonymous access
        public async Task<IEnumerable<ReplyDto>> GetRepliesOfPost(Guid postId)
        {
            if (postId == Guid.Empty) throw new ArgumentNullException(nameof(postId));
            
            var post = _dbContext.Posts.Where(x => x.Id == postId).FirstOrDefault();

            List<Reply> replies = await dbSet.Where(r => r.PostId == postId).ToListAsync();

            List<ReplyDto> replyDtos = new();

            if (replies.Count > 0)
            {
                foreach (var reply in replies)
                {
                    User user = await _dbContext.Users.Where(x => x.Id == reply.UserId).FirstAsync();
                    ReplyDto dto = new ReplyDto()
                    {
                        Id = reply.Id,
                        PostId = postId,
                        UserId = reply.UserId,
                        Content = reply.Content,
                        Username = user.Name,
                        ProfileImage = user.ImageData,
                        DateOfCreation = reply.DateOfCreation
                    };
                    replyDtos.Add(dto);
                }
            }
          

            var repliesInOrder = replyDtos.OrderBy(x => x.DateOfCreation);
            return repliesInOrder; 
        }


        //throw error
        public async Task<int> GetRepliesCount(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            List<Reply> result = await dbSet.Where(r => r.UserId == userId).ToListAsync();
            return result.Count;

        }


        public async Task<ReplyDto> AddReply(Reply reply)
        {
            if (reply == null) throw new ArgumentNullException(nameof(reply));

            if (_userContext.HasAuthorization(reply.UserId))
            {

                reply.Id = Guid.NewGuid();
                var added = await dbSet.AddAsync(reply);
                ReplyDto dto = new();

                if (added != null)
                {
                    Post post = await _dbContext.Posts.Where(p => p.Id == reply.PostId).FirstAsync();
                    EmailData emailData = new()
                    {

                        Recipicent = "ryann.rempel@ethereal.email",
                        Subject = "New Reply",
                        DataName = post.Title,
                        Url = "http://localhost:4200/post/" + post.Id,
                        Body = "2dl víz"
                    };

                    notificationService.SendEmail(emailData, EmailTemplate.NEWREPLY);

                    User user = await _dbContext.Users.Where(p => p.Id == reply.UserId).FirstAsync();
                    dto = new()
                    {
                        Id = reply.Id,
                        UserId = reply.UserId,
                        PostId = reply.PostId,
                        Content = reply.Content,
                        DateOfCreation = reply.DateOfCreation,
                        Username = user.Name,
                        ProfileImage = user.ImageData
                    };
                   // return dto;
                }

                return dto;
            }
            else
            {
                throw new UnauthorizedAccessException("The logged in user has no access.");
            }
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

            Reply toBeDeleted = await dbSet.Where(r => r.Id == replyId).FirstAsync();

            if (toBeDeleted != null && _userContext.HasAuthorization(toBeDeleted.UserId))
            {
                var result = dbSet.Remove(toBeDeleted);
                return result.State == EntityState.Deleted;
            }
            return false;
        }
    }
}
