using CwkSocial.APPLICATION.Models;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.APPLICATION.Posts.Queries
{
    public class GetPostCommentByIdToPostQuery : IRequest<OperationResult<PostComment>>
    {
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }
    }
}