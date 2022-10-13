using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using MediatR;

namespace CwkSocial.APPLICATION.UserProfiles.Queries
{
    public class GetAllUserProfileQuery : IRequest<IEnumerable<UserProfile>>
    {
    }
}