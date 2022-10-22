using CwkSocial.APPLICATION.Models;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.APPLICATION.Posts.Commands
{
    public class AddPostInteractionToPostCommand :IRequest<OperationResult<PostInteraction>>
    {
        public Guid PostId { get; set; }
        public InteractionType InteractionType { get; set; }
    }
}