using CwkSocial.APPLICATION.Identity.Commands;
using CwkSocial.APPLICATION.Identity.Dtos;
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
    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, OperationResult<AuthenticationIdentityUserDto>>
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

        public async Task<OperationResult<AuthenticationIdentityUserDto>> Handle(RegisterCommand request,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<AuthenticationIdentityUserDto>();

            try
            {
                await ValidateIdentityDoesNotExist(result, request);

                if (result.IsError) return result;

                await using var transaction = await _dataContext.Database.BeginTransactionAsync(cancellationToken);

                var identityUser = await CreateIdentityUserAsync(result, request, transaction, cancellationToken);

                if (result.IsError) return result;

                var userProfile = await CreateUserProfileAsync(request, transaction, identityUser, cancellationToken);

                await transaction.CommitAsync(cancellationToken);


                var authenticationIdentityUser = new AuthenticationIdentityUserDto()
                {
                    Username = identityUser.UserName,
                    FirstName = userProfile.BasicInfo.FirstName,
                    LastName = userProfile.BasicInfo.LastName,
                    DateOfBirth = userProfile.BasicInfo.DateOfBirth,
                    PhoneNumber = userProfile.BasicInfo.PhoneNumber,
                    CurrentCity = userProfile.BasicInfo.CurrentCity,
                    Token = GetJwtString(identityUser, userProfile)
                };
               
                result.PayLoad = authenticationIdentityUser;
            }
            catch (UserProfileValidateException ex)
            {
                result.AddError(ErrorCode.ValidationError, ex.Message);
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.UnknowError, ex.Message);
            }

            return result;
        }

        private async Task ValidateIdentityDoesNotExist(OperationResult<AuthenticationIdentityUserDto> result, RegisterCommand request)
        {
            var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

            if (existingIdentity is not null)
            {
                result.AddError(ErrorCode.IdentityUserAlreadyExists, IdentityErrorMessage.IdentityUserAlreadyExists);
            }
        }

        private async Task<IdentityUser> CreateIdentityUserAsync(OperationResult<AuthenticationIdentityUserDto> result,
            RegisterCommand request, IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            var identityUser = new IdentityUser() { Email = request.Username, UserName = request.Username };

            var createIdentity = await _userManager.CreateAsync(identityUser, request.Password);

            if (!createIdentity.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);

                foreach (var identityError in createIdentity.Errors)
                {
                    result.AddError(ErrorCode.IdentityCreationFailed, identityError.Description);
                }
            }

            return identityUser;
        }

        private async Task<UserProfile> CreateUserProfileAsync(RegisterCommand request, IDbContextTransaction transaction,
            IdentityUser identityUser, CancellationToken cancellationToken)
        {
            try
            {
                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username,
                    request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

                var userProfile = UserProfile.CreateUserProfile(identityUser.Id, basicInfo);

                _dataContext.UserProfiles.Add(userProfile);
                await _dataContext.SaveChangesAsync(cancellationToken);

                return userProfile;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw ex;
            }
        }

        private string GetJwtString(IdentityUser identityUser, UserProfile userProfile)
        {
            var claimIndetity = new ClaimsIdentity(new Claim[]
               {
                    new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
                    new Claim("IdentityId", identityUser.Id),
                    new Claim("UserProfileId", userProfile.UserProfileId.ToString())
               });

            var token = _identityService.CreateSecurityToken(claimIndetity);
            return _identityService.WriteToken(token);
        }
    }
}