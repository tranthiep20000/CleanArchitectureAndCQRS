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
                    var error = new Error()
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No Post with ID {request.PostId}"
                    };

                    result.IsError = true;
                    result.Errors.Add(error);

                    return result;
                }

                var postComment = post.Comments.FirstOrDefault(comment => comment.CommentId == request.CommentId);

                if (postComment is null)
                {
                    var error = new Error
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No PostComment with ID {request.CommentId}"
                    };

                    result.IsError = true;
                    result.Errors.Add(error);

                    return result;
                }

                post.RemovePostComment(postComment);

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