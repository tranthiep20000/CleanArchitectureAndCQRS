using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Posts.Queries;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Posts.QueryHandlers
{
    internal class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, OperationResult<IEnumerable<Post>>>
    {
        private readonly DataContext _dataContext;

        public GetAllPostsQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<IEnumerable<Post>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<Post>>();

            try
            {
                var posts = await _dataContext.Posts.ToListAsync();
                result.PayLoad = posts;
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