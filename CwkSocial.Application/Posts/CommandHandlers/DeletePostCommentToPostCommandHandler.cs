using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Commands;
using CwkSocial.DAL.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.CommandHandlers
{
    internal class DeletePostCommentToPostCommandHandler : IRequestHandler<DeletePostCommentToPostCommand, OperationResult<bool>>
    {
        private readonly DataContext _dataContext;

        public DeletePostCommentToPostCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<OperationResult<bool>> Handle(DeletePostCommentToPostCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<bool>();

            try
            {
                var post = await _dataContext.Posts
                    .Include(postComments => postComments.Comments)
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId);

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

                post.RemovePostComment(postComment);

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