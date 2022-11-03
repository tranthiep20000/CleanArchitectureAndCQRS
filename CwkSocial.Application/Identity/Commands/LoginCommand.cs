using CwkSocial.APPLICATION.Identity.Dtos;
using CwkSocial.APPLICATION.Models;
using MediatR;

namespace CwkSocial.APPLICATION.Identity.Commands
{
    public class LoginCommand : IRequest<OperationResult<AuthenticationIdentityUserDto>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}