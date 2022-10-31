using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Queries;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.QueryHandlers
{
    internal class GetPostCommentByIdToPostQueryHandler : IRequestHandler<GetPostCommentByIdToPostQuery, OperationResult<PostComment>>
    {
        private readonly DataContext _dataContext;

        public GetPostCommentByIdToPostQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<PostComment>> Handle(GetPostCommentByIdToPostQuery request,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostComment>();

            try
            {
                var post = await _dataContext.Posts
                    .Include(postComments => postComments.Comments)
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId);

                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessage.PostNotFound, request.PostId));

                    return result;
                }

                var postComment = post.Comments.FirstOrDefault(comment => comment.CommentId == request.CommentId);

                if (postComment is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessage.PostCommentNotFound, request.CommentId));

                    return result;
                }

                result.PayLoad = postComment;
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.UnknowError, ex.Message);
            }

            return result;
        }
    }
}