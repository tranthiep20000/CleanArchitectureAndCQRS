using CwkSocial.APPLICATION.UserProfiles.Queries;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.UserProfiles.QueryHandlers
{
    internal class GetAllUserProfileQueryHandler : IRequestHandler<GetAllUserProfileQuery, IEnumerable<UserProfile>>
    {
        private readonly DataContext _dataContext;

        public GetAllUserProfileQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<UserProfile>> Handle(GetAllUserProfileQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.UserProfiles.ToListAsync();
        }
    }
}