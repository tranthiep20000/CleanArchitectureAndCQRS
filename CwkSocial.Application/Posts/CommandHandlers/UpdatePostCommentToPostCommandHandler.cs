using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Commands;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.CommandHandlers
{
    internal class UpdatePostCommentToPostCommandHandler : IRequestHandler<UpdatePostCommentToPostCommand, OperationResult<bool>>
    {
        private readonly DataContext _dataContext;

        public UpdatePostCommentToPostCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<bool>> Handle(UpdatePostCommentToPostCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<bool>();

            try
            {
                var post = await _dataContext.Posts
                    .Include(postComments => postComments.Comments)
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessage.PostNotFound, request.PostId));

                    return result;
                }

                var postComment = post.Comments.FirstOrDefault(comment => comment.CommentId == request.CommentId);

                if (postComment is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessage.PostCommentNotFound, request.CommentId));

                    return result;
                }

                postComment.UpdateCommentText(request.TextComment);
                // TO DO: no update post comment in post 
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