using CwkSocial.APPLICATION.Identity.Dtos;
using CwkSocial.APPLICATION.Identity.Queries;
using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Services;
using CwkSocial.APPLICATION.UserProfiles;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CwkSocial.APPLICATION.Identity.QueryHandlers
{
    internal class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, OperationResult<AuthenticationIdentityUserDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IdentityService _identityService;

        public GetCurrentUserQueryHandler(DataContext dataContext, IdentityService identityService)
        {
            _dataContext = dataContext;
            _identityService = identityService;
        }

        public async Task<OperationResult<AuthenticationIdentityUserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<AuthenticationIdentityUserDto>();

            try
            {
                var identityUser = await _dataContext.Users
                    .FirstOrDefaultAsync(user => user.Id == request.IdentityId.ToString(), cancellationToken);

                if (identityUser is null)
                {
                    result.AddError(ErrorCode.IdentityUserDoesNotExsist, IdentityErrorMessage.IdentityUserDoesNotExsist);

                    return result;
                }

                var userProfile = await _dataContext.UserProfiles
                    .FirstOrDefaultAsync(userProfile => userProfile.IdentityId == request.IdentityId.ToString(), cancellationToken);

                if (userProfile is null)
                {
                    result.AddError(ErrorCode.NotFound, UserProfileErrorMessage.UserProfileNotFound);

                    return result;
                }

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
            catch (Exception ex)
            {
                result.AddError(ErrorCode.UnknowError, ex.Message);
            }

            return result;
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