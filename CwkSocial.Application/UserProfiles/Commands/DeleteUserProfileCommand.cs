using CwkSocial.APPLICATION.Models;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.APPLICATION.UserProfiles.Commands
{
    public class DeleteUserProfileCommand : IRequest<OperationResult<bool>>
    {
        public Guid UserProfileId { get; set; }
    }
}
