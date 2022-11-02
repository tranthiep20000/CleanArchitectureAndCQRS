using CwkSocial.APPLICATION.Models;
using MediatR;

namespace CwkSocial.APPLICATION.Identity.Commands
{
    public class RemoveAccountCommand :IRequest<OperationResult<bool>>
    {
        public Guid IdentityId { get; set; }
    }
}