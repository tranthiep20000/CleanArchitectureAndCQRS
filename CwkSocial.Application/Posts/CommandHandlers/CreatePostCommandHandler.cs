using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Commands;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using CwkSocial.DOMAIN.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.CommandHandlers
{
    internal class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, OperationResult<Post>>
    {
        private readonly DataContext _dataContext;

        public CreatePostCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<Post>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();

            try
            {
                var userProfile = await _dataContext.UserProfiles
                    .FirstOrDefaultAsync(userProfile => userProfile.UserProfileId == request.UserProfileId, cancellationToken);

                if (userProfile is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessage.UserProfileNotFound, request.UserProfileId));

                    return result;
                }

                var post = Post.CreatePost(request.UserProfileId, request.TextContent);

                _dataContext.Posts.Add(post);
                await _dataContext.SaveChangesAsync(cancellationToken);

                result.PayLoad = post;
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