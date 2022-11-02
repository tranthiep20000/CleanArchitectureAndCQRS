using CwkSocial.APPLICATION.Models;
using MediatR;

namespace CwkSocial.APPLICATION.Posts.Commands
{
    public class DeletePostInteractionToPostCommand :IRequest<OperationResult<bool>>
    {
        public Guid PostId { get; set; }
        public Guid InteractionId { get; set; }
        public Guid UserProfileId { get; set; }
    }
}