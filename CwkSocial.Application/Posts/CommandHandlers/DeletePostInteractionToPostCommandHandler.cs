using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Commands;
using CwkSocial.DAL.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.CommandHandlers
{
    internal class DeletePostInteractionToPostCommandHandler : IRequestHandler<DeletePostInteractionToPostCommand, OperationResult<bool>>
    {
        private readonly DataContext _dataContext;

        public DeletePostInteractionToPostCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<bool>> Handle(DeletePostInteractionToPostCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<bool>();

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

                post.RemovePostInteraction(postInteraction);

                _dataContext.Posts.Update(post);
                await _dataContext.SaveChangesAsync();

                result.PayLoad = true;
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