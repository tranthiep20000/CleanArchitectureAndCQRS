using CwkSocial.APPLICATION.Models;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.APPLICATION.Posts.Queries
{
    public class GetAllPostsQuery : IRequest<OperationResult<IEnumerable<Post>>>
    {
    }
}