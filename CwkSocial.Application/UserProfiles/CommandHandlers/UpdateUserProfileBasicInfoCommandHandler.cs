using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.UserProfiles.Commands;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using CwkSocial.DOMAIN.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.UserProfiles.CommandHandlers
{
    internal class UpdateUserProfileBasicInfoCommandHandler : IRequestHandler<UpdateUserProfileBasicInfoCommand, OperationResult<bool>>
    {
        private readonly DataContext _dataContext;

        public UpdateUserProfileBasicInfoCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<bool>> Handle(UpdateUserProfileBasicInfoCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<bool>();

            try
            {
                var userProfile = await _dataContext.UserProfiles.
                FirstOrDefaultAsync(userProfile => userProfile.UserProfileId == request.UserProfileId);

                if (userProfile is null)
                {
                    var error = new Error
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No UserProfile with ID {request.UserProfileId}"
                    };

                    result.IsError = true;
                    result.Errors.Add(error);

                    return result;
                }

                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress,
                    request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

                userProfile.UpdateBasicInfo(basicInfo);

                _dataContext.UserProfiles.Update(userProfile);
                await _dataContext.SaveChangesAsync();

                result.PayLoad = true;
            }
            catch(UserProfileValidateException ex)
            {
                result.IsError = true;

                ex.ValidationErrors.ForEach(e =>
                {
                    var error = new Error()
                    {
                        Code = ErrorCode.ValidationError,
                        Message = $"{ex.Message}"
                    };

                    result.Errors.Add(error);
                });
            }
            catch (Exception ex)
            {
                var error = new Error
                {
                    Code = ErrorCode.UnknowError,
                    Message = ex.Message
                };

                result.IsError = true;
                result.Errors.Add(error);
            }

            return result;
        }
    }
}