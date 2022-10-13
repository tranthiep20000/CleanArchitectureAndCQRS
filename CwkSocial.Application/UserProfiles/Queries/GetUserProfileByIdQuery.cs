using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.APPLICATION.UserProfiles.Queries
{
    public class GetUserProfileByIdQuery : IRequest<UserProfile>
    {
        public Guid UserProfileId { get; set; }
    }
}
