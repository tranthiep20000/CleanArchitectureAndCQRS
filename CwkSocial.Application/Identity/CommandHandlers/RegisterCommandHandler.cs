using CwkSocial.APPLICATION.Identity.Commands;
using CwkSocial.APPLICATION.Models;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using CwkSocial.DOMAIN.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CwkSocial.APPLICATION.Identity.CommandHandlers
{
    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, OperationResult<string>>
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<IdentityUser> _userManager;
        public RegisterCommandHandler(DataContext dataContext, UserManager<IdentityUser> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<OperationResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<string>();

            try
            {
                var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

                if (existingIdentity is not null)
                {
                    var error = new Error()
                    {
                        Code = ErrorCode.IdentityUserAlreadyExists,
                        Message = "Provided email address already exsits. Cannot register new user"
                    };

                    result.IsError = true;
                    result.Errors.Add(error);

                    return result;
                }

                var identity = new IdentityUser()
                {
                    Email = request.Username,
                    UserName = request.Username
                };

                using var transaction = _dataContext.Database.BeginTransaction();

                var createIdentity = await _userManager.CreateAsync(identity);
                
                if (createIdentity.Succeeded)
                {
                    result.IsError = true;

                    foreach (var identityError in createIdentity.Errors)
                    {
                        var error = new Error()
                        {
                            Code = ErrorCode.IdentityCreationFailed,
                            Message = $"{identityError.Description}"
                        };

                        result.Errors.Add(error);
                    }

                    return result;
                }

                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username,
                    request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

                var userProfile = UserProfile.CreateUserProfile(identity.Id, basicInfo);
            }
            catch (UserProfileValidateException ex)
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
                var error = new Error()
                {
                    Code = ErrorCode.UnknowError,
                    Message = $"{ex.Message}"
                };

                result.IsError = true;
                result.Errors.Add(error);
            }

            return result;
        }
    }
}