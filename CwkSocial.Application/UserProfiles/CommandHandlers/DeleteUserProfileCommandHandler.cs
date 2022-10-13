using CwkSocial.APPLICATION.UserProfiles.Commands;
using CwkSocial.DAL.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.UserProfiles.CommandHandlers
{
    internal class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand, bool>
    {
        private readonly DataContext _dataContext;

        public DeleteUserProfileCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _dataContext.UserProfiles
                .FirstOrDefaultAsync(userProfile => userProfile.UserProfileId == request.UserProfileId);

            _dataContext.UserProfiles.Remove(userProfile);
            await _dataContext.SaveChangesAsync();

            return true;
        }
    }
}