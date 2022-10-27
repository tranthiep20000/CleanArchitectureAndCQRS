using CwkSocial.APPLICATION.Identity.Commands;
using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.Options;
using CwkSocial.DAL.Data;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using CwkSocial.DOMAIN.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CwkSocial.APPLICATION.Identity.CommandHandlers
{
    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, OperationResult<string>>
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public RegisterCommandHandler(DataContext dataContext, UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
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

                // creating transaction
                using var transaction = _dataContext.Database.BeginTransaction();

                var createIdentity = await _userManager.CreateAsync(identity);

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

                    return result;
                }

                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username,
                    request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

                var userProfile = UserProfile.CreateUserProfile(identity.Id, basicInfo);

                try
                {
                    _dataContext.UserProfiles.Add(userProfile);
                    await _dataContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                        new Claim("IdentityId", identity.Id),
                        new Claim("UserProfileId", userProfile.UserProfileId.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    Audience = _jwtSettings.Audiences[0],
                    Issuer = _jwtSettings.Issuer,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                result.PayLoad = tokenHandler.WriteToken(token);
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