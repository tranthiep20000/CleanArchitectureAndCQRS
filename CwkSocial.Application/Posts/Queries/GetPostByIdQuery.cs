using CwkSocial.APPLICATION.Models;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.APPLICATION.Posts.Queries
{
    public class GetPostByIdQuery : IRequest<OperationResult<Post>>
    {
        public Guid PostId { get; set; }
    }
}