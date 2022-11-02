using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Commands;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using CwkSocial.DOMAIN.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.CommandHandlers
{
    internal class AddPostCommentToPostCommandHandler : IRequestHandler<AddPostCommentToPostCommand, OperationResult<PostComment>>
    {
        private readonly DataContext _dataContext;

        public AddPostCommentToPostCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<PostComment>> Handle(AddPostCommentToPostCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostComment>();

            try
            {
                var post = await _dataContext.Posts
                    .FirstOrDefaultAsync(post => post.PostId == request.PostId, cancellationToken);

                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessage.PostNotFound, request.PostId));

                    return result;
                }

                var userProfile = await _dataContext.UserProfiles
                    .FirstOrDefaultAsync(userProfile => userProfile.UserProfileId == request.UserProfileId, cancellationToken);

                if (userProfile is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessage.UserProfileNotFound, request.UserProfileId));

                    return result;
                }

                var postComment = PostComment.CreatePostComment(request.PostId, request.TextComment, request.UserProfileId);

                post.AddPostComment(postComment);

                _dataContext.Posts.Update(post);
                await _dataContext.SaveChangesAsync(cancellationToken);

                result.PayLoad = postComment;
            }
            catch (PostCommentValidateException ex)
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