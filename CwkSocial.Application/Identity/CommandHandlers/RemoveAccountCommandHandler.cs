using CwkSocial.APPLICATION.Identity.Commands;
using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.UserProfiles;
using CwkSocial.DAL.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.Identity.CommandHandlers
{
    internal class RemoveAccountCommandHandler : IRequestHandler<RemoveAccountCommand, OperationResult<bool>>
    {
        private readonly DataContext _dataContext;

        public RemoveAccountCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<bool>> Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<bool>();

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

                _dataContext.UserProfiles.Remove(userProfile);
                _dataContext.Users.Remove(identityUser);
                await _dataContext.SaveChangesAsync(cancellationToken);

                result.PayLoad = true;
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.UnknowError, ex.Message);
            }

            return result;
        }
    }
}