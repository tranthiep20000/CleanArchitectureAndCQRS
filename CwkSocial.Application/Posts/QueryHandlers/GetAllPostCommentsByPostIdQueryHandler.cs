using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Queries;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.QueryHandlers
{
    internal class GetAllPostCommentsByPostIdQueryHandler : IRequestHandler<GetAllPostCommentsByPostIdQuery,
        OperationResult<IEnumerable<PostComment>>>
    {
        private readonly DataContext _dataContext;

        public GetAllPostCommentsByPostIdQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<IEnumerable<PostComment>>> Handle(GetAllPostCommentsByPostIdQuery request,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<PostComment>>();

            try
            {
                var post = await _dataContext.Posts
                    .Include(post => post.Comments)
                    .FirstOrDefaultAsync(post => post.PostId == request.PostId);

                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessage.PostNotFound, request.PostId));

                    return result;
                }

                result.PayLoad = post.Comments;
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.UnknowError, ex.Message);
            }

            return result;
        }
    }
}