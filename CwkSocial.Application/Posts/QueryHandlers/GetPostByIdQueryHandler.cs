using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Queries;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.QueryHandlers
{
    internal class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, OperationResult<Post>>
    {
        private readonly DataContext _dataContext;
        public GetPostByIdQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();

            try
            {
                var post = await _dataContext.Posts.
                    FirstOrDefaultAsync(post => post.PostId == request.PostId);

                if (post is null)
                {
                    var error = new Error
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No Post with ID {request.PostId}"
                    };

                    result.IsError = true;
                    result.Errors.Add(error);

                    return result;
                }
            }
            catch (Exception ex)
            {
                var error = new Error
                {
                    Code = ErrorCode.UnknowError,
                    Message = ex.Message
                };

                result.IsError = true;
                result.Errors.Add(error);
            }

            return result;
        }
    }
}