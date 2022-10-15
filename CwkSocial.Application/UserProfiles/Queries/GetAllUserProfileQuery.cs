using CwkSocial.APPLICATION.Models;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using MediatR;

namespace CwkSocial.APPLICATION.UserProfiles.Queries
{
    public class GetAllUserProfileQuery : IRequest<OperationResult<IEnumerable<UserProfile>>>
    {
    }
}