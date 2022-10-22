using CwkSocial.APPLICATION.Models;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.APPLICATION.Posts.Queries
{
    public class GetAllPostCommentsByPostIdQuery : IRequest<OperationResult<IEnumerable<PostComment>>>
    {
        public Guid PostId { get; set; }
    }
}