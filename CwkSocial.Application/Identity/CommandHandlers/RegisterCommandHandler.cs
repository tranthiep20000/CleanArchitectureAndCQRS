using CwkSocial.APPLICATION.Identity.Commands;
using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Services;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using CwkSocial.DOMAIN.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CwkSocial.APPLICATION.Identity.CommandHandlers
{
    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, OperationResult<string>>
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityService _identityService;

        public RegisterCommandHandler(DataContext dataContext, UserManager<IdentityUser> userManager, IdentityService identityService)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _identityService = identityService;
        }

        public async Task<OperationResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<string>();

            try
            {
                var creationValidated = await ValidateIdentityDoesNotExist(result, request);

                if (!creationValidated) return result;

                using var transaction = await _dataContext.Database.BeginTransactionAsync();

                var identityUser = await CreateIdentityUserAsync(result, request, transaction);

                if (identityUser is null) return result;

                var userProfile = await CreateUserProfileAsync(request, transaction, identityUser);

                await transaction.CommitAsync();

                var claimIndetity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
                    new Claim("IdentityId", identityUser.Id),
                    new Claim("UserProfileId", userProfile.UserProfileId.ToString())
                });

                var token = _identityService.CreateSecurityToken(claimIndetity);
                result.PayLoad = _identityService.WriteToken(token);
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

        private async Task<bool> ValidateIdentityDoesNotExist(OperationResult<string> result, RegisterCommand request)
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

                return false;
            }

            return true;
        }

        private async Task<IdentityUser> CreateIdentityUserAsync(OperationResult<string> result, RegisterCommand request, IDbContextTransaction transaction)
        {
            var identityUser = new IdentityUser() { Email = request.Username, UserName = request.Username };

            var createIdentity = await _userManager.CreateAsync(identityUser);

            if (!createIdentity.Succeeded)
            {
                await transaction.RollbackAsync();
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

                return null;
            }

            return identityUser;
        }

        private async Task<UserProfile> CreateUserProfileAsync(RegisterCommand request, IDbContextTransaction transaction, IdentityUser identityUser)
        {
            try
            {
                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username,
                    request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

                var userProfile = UserProfile.CreateUserProfile(identityUser.Id, basicInfo);

                _dataContext.UserProfiles.Add(userProfile);
                await _dataContext.SaveChangesAsync();

                return userProfile;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}