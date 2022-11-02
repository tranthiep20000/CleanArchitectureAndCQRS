using CwkSocial.APPLICATION.Models;
using MediatR;

namespace CwkSocial.APPLICATION.Posts.Commands
{
    public class UpdatePostCommentToPostCommand : IRequest<OperationResult<bool>>
    {
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }
        public Guid UserProfileId { get; set; }
        public string TextComment { get; set; }
    }
}