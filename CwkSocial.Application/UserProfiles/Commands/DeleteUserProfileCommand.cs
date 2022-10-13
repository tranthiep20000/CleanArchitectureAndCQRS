using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.APPLICATION.UserProfiles.Commands
{
    public class DeleteUserProfileCommand : IRequest<bool>
    {
        public Guid UserProfileId { get; set; }
    }
}
