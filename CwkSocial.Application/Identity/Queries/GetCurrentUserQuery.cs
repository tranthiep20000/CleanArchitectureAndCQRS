using CwkSocial.APPLICATION.Identity.Dtos;
using CwkSocial.APPLICATION.Models;
using MediatR;

namespace CwkSocial.APPLICATION.Identity.Queries
{
    public class GetCurrentUserQuery : IRequest<OperationResult<AuthenticationIdentityUserDto>>
    {
        public Guid IdentityId { get; set; }
    }
}