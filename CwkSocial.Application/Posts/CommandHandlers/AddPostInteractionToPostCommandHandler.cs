using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Commands;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using CwkSocial.DOMAIN.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.CommandHandlers
{
    internal class AddPostInteractionToPostCommandHandler : IRequestHandler<AddPostInteractionToPostCommand,
        OperationResult<PostInteraction>>
    {
        private readonly DataContext _dataContext;

        public AddPostInteractionToPostCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<PostInteraction>> Handle(AddPostInteractionToPostCommand request,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostInteraction>();

            try
            {
                var post = await _dataContext.Posts
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId);

                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessage.PostNotFound, request.PostId));

                    return result;
                }

                var postInteraction = PostInteraction.CreatePostInteraction(request.PostId, request.InteractionType);

                post.AddPostInteraction(postInteraction);

                _dataContext.Posts.Update(post);
                await _dataContext.SaveChangesAsync(cancellationToken);

                result.PayLoad = postInteraction;
            }
            catch (PostInteractionValidateException ex)
            {
                ex.ValidationErrors.ForEach(e => result.AddError(ErrorCode.ValidationError, e));
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.UnknowError, ex.Message);
            }

            return result;
        }
    }
}