using CwkSocial.APPLICATION.UserProfiles.Commands;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using MediatR;

namespace CwkSocial.APPLICATION.UserProfiles.CommandHandlers
{
    internal class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, UserProfile>
    {
        private readonly DataContext _dataContext;

        public CreateUserProfileCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<UserProfile> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName,
                    request.EmailAddress, request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

            var userProfile = UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), basicInfo);

            _dataContext.UserProfiles.Add(userProfile);
            await _dataContext.SaveChangesAsync();

            return userProfile;
        }
    }
}