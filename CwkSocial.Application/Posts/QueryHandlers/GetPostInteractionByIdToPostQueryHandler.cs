using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Queries;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.QueryHandlers
{
    internal class GetPostInteractionByIdToPostQueryHandler : IRequestHandler<GetPostInteractionByIdToPostQuery, OperationResult<PostInteraction>>
    {
        private readonly DataContext _dataContext;

        public GetPostInteractionByIdToPostQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<PostInteraction>> Handle(GetPostInteractionByIdToPostQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostInteraction>();

            try
            {
                var post = await _dataContext.Posts
                    .Include(postInteractions => postInteractions.Interactions)
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId);

                if (post is null)
                {
                    var error = new Error()
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No Post with ID {request.PostId}"
                    };

                    result.IsError = true;
                    result.Errors.Add(error);

                    return result;
                }

                var postInteraction = post.Interactions.FirstOrDefault(interaction => interaction.InteractionId == request.InteractionId);

                if (postInteraction is null)
                {
                    var error = new Error()
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No PostInteraction with ID {request.InteractionId}"
                    };

                    result.IsError = true;
                    result.Errors.Add(error);

                    return result;
                }

                result.PayLoad = postInteraction;
            }
            catch (Exception ex)
            {
                var error = new Error()
                {
                    Code = ErrorCode.UnknowError,
                    Message = $"{ex.Message}"
                };

                result.IsError = true;
                result.Errors.Add(error);
            }

            return result;
        }
    }
}