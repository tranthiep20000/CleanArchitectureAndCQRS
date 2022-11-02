using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Commands;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
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
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessage.PostNotFound, request.PostId));

                    return result;
                }

                var postInteraction = post.Interactions.FirstOrDefault(interaction => interaction.InteractionId == request.InteractionId);

                if (postInteraction is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessage.PostInteractionNotFound, request.InteractionId));
                    return result;
                }

                if (postInteraction.UserProfileId != request.UserProfileId)
                {
                    result.AddError(ErrorCode.InteractionDeleteNotPossible, PostErrorMessage.InteractionDeleteNotPossible);

                    return result;
                }

                post.RemovePostInteraction(postInteraction);

                _dataContext.Posts.Update(post);
                await _dataContext.SaveChangesAsync(cancellationToken);

                result.PayLoad = true;
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.UnknowError, ex.Message);
            }

            return result;
        }
    }
}