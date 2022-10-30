using CwkSocial.APPLICATION.Models;
using MediatR;

namespace CwkSocial.APPLICATION.Posts.Commands
{
    public class DeletePostCommand : IRequest<OperationResult<bool>>
    {
        public Guid PostId { get; set; }
        public Guid UserProfileId { get; set; }
    }
}