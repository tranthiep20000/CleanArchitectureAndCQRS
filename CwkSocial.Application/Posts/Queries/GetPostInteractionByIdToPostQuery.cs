using CwkSocial.APPLICATION.Models;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.APPLICATION.Posts.Queries
{
    public class GetPostInteractionByIdToPostQuery : IRequest<OperationResult<PostInteraction>>
    {
        public Guid PostId { get; set; }
        public Guid InteractionId { get; set; }
    }
}