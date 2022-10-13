using CwkSocial.APPLICATION.UserProfiles.Commands;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.UserProfiles.CommandHandlers
{
    internal class UpdateUserProfileBasicInfoCommandHandler : IRequestHandler<UpdateUserProfileBasicInfoCommand, bool>
    {
        private readonly DataContext _dataContext;

        public UpdateUserProfileBasicInfoCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> Handle(UpdateUserProfileBasicInfoCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _dataContext.UserProfiles.
                FirstOrDefaultAsync(userProfile => userProfile.UserProfileId == request.UserProfileId);

            var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress,
                request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

            userProfile.UpdateBasicInfo(basicInfo);

            _dataContext.UserProfiles.Update(userProfile);
            await _dataContext.SaveChangesAsync();

            return true;
        }
    }
}