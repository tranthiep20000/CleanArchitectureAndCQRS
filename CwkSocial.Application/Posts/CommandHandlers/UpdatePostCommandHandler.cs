using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Commands;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.CommandHandlers
{
    internal class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, OperationResult<bool>>
    {
        private readonly DataContext _dataContext;

        public UpdatePostCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<bool>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<bool>();

            try
            {
                var post = await _dataContext.Posts
                    .FirstOrDefaultAsync(post => post.PostId == request.PostId, cancellationToken);

                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessage.PostNotFound, request.PostId));

                    return result;
                }

                if (post.UserProfileId != request.UserProfileId)
                {
                    result.AddError(ErrorCode.PostUpdateNotPossible, PostErrorMessage.PostUpdateNotPossible);

                    return result;
                }

                post.UpdatePostText(request.TextContent);

                _dataContext.Update(post);
                await _dataContext.SaveChangesAsync(cancellationToken);

                result.PayLoad = true;
            }
            catch (PostValidateException ex)
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