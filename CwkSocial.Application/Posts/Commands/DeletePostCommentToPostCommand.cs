using CwkSocial.APPLICATION.Models;
using MediatR;

namespace CwkSocial.APPLICATION.Posts.Commands
{
    public class DeletePostCommentToPostCommand : IRequest<OperationResult<bool>>
    {
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }
        public Guid UserProfileId { get; set; }
    }
}