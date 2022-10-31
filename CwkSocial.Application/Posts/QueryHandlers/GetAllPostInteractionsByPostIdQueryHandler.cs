using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Queries;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.QueryHandlers
{
    internal class GetAllPostInteractionsByPostIdQueryHandler : IRequestHandler<GetAllPostInteractionsByPostIdQuery,
        OperationResult<IEnumerable<PostInteraction>>>
    {
        private readonly DataContext _dataContext;

        public GetAllPostInteractionsByPostIdQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<IEnumerable<PostInteraction>>> Handle(GetAllPostInteractionsByPostIdQuery request,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<PostInteraction>>();

            try
            {
                var post = await _dataContext.Posts
                    .Include(postInteractions => postInteractions.Interactions)
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId);

                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessage.PostNotFound, request.PostId));

                    return result;
                }

                result.PayLoad = post.Interactions;
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.UnknowError, ex.Message);
            }

            return result;
        }
    }
}