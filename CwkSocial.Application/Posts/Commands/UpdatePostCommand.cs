using CwkSocial.APPLICATION.Models;
using MediatR;

namespace CwkSocial.APPLICATION.Posts.Commands
{
    public class UpdatePostCommand : IRequest<OperationResult<bool>>
    {
        public Guid PostId { get; set; }
        public string TextContent { get; set; }
        public Guid UserProfileId { get; set; }
    }
}