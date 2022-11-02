using CwkSocial.APPLICATION.Identity.Commands;
using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Services;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CwkSocial.APPLICATION.Identity.CommandHandlers
{
    internal class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<string>>
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityService _identityService;

        public LoginCommandHandler(DataContext dataContext, UserManager<IdentityUser> userManager, IdentityService identityService)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _identityService = identityService;
        }

        public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<string>();

            try
            {
                var identityUser = await ValidateAndGetIdentityAsync(request, result);

                if (result.IsError) return result;

                var userProfile = await _dataContext.UserProfiles
                    .FirstOrDefaultAsync(userProfile => userProfile.IdentityId == identityUser.Id, cancellationToken);

                result.PayLoad = GetJwtString(identityUser, userProfile);
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.UnknowError, ex.Message);
            }

            return result;
        }

        private async Task<IdentityUser> ValidateAndGetIdentityAsync(LoginCommand request, OperationResult<string> result)
        {
            var identityUser = await _userManager.FindByEmailAsync(request.Username);

            if (identityUser is null)
            {
                result.AddError(ErrorCode.IdentityUserDoesNotExsist, IdentityErrorMessage.IdentityUserDoesNotExsist);
            }

            var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

            if (!validPassword)
            {
                result.AddError(ErrorCode.IncorrectPassword, IdentityErrorMessage.IncorrectPassword);
            }

            return identityUser;
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