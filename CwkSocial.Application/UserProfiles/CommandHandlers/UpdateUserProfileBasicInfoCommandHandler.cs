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
                FirstOrDefaultAsync(userProfile => userProfile.UserProfileId == request.UserProfileId, cancellationToken);

                if (userProfile is null)
                {
                    result.AddError(ErrorCode.NotFound, string.Format(UserProfileErrorMessage.UserProfileNotFound, request.UserProfileId));

                    return result;
                }

                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress,
                    request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

                userProfile.UpdateBasicInfo(basicInfo);

                _dataContext.UserProfiles.Update(userProfile);
                await _dataContext.SaveChangesAsync(cancellationToken);

                result.PayLoad = true;
            }
            catch(UserProfileValidateException ex)
            {
                ex.ValidationErrors.ForEach(e => result.AddError(ErrorCode.ValidationError, e));
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.UnknowError, ex.Message);
            }

            return result;
        }
    }
}