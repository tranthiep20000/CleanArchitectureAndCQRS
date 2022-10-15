using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.UserProfiles.Commands;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using MediatR;

namespace CwkSocial.APPLICATION.UserProfiles.CommandHandlers
{
    internal class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, OperationResult<UserProfile>>
    {
        private readonly DataContext _dataContext;

        public CreateUserProfileCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<UserProfile>> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();

            try
            {
                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName,
                    request.EmailAddress, request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

                var userProfile = UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), basicInfo);

                _dataContext.UserProfiles.Add(userProfile);
                await _dataContext.SaveChangesAsync();

                result.PayLoad = userProfile;
                return result;
            }
            catch (Exception ex)
            {
                var error = new Error
                {
                    Code = ErrorCode.ServerError,
                    Message = ex.Message
                };

                result.IsError = true;
                result.Errors.Add(error);

                return result;
            }
        }
    }
}