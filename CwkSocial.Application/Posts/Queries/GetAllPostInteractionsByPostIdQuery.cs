using CwkSocial.APPLICATION.Models;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.APPLICATION.Posts.Queries
{
    public class GetAllPostInteractionsByPostIdQuery : IRequest<OperationResult<IEnumerable<PostInteraction>>>
    {
        public Guid PostId { get; set; }
    }
}