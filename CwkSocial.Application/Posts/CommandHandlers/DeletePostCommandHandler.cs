using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Commands;
using CwkSocial.DAL.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.CommandHandlers
{
    internal class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, OperationResult<bool>>
    {
        private readonly DataContext _dataContext;

        public DeletePostCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<bool>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
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
                    result.AddError(ErrorCode.PostDeleteNotPossible, PostErrorMessage.PostDeleteNotPossible);

                    return result;
                }

                _dataContext.Posts.Remove(post);
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