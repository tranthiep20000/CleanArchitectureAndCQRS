using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.UserProfiles.Queries;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.UserProfiles.QueryHandlers
{
    internal class GetAllUserProfileQueryHandler : IRequestHandler<GetAllUserProfileQuery, OperationResult<IEnumerable<UserProfile>>>
    {
        private readonly DataContext _dataContext;

        public GetAllUserProfileQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfileQuery request,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<UserProfile>>();

            try
            {
                var userProfiles =  await _dataContext.UserProfiles.ToListAsync();

                result.PayLoad = userProfiles;
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.UnknowError, ex.Message);
            }

            return result;
        }
    }
}