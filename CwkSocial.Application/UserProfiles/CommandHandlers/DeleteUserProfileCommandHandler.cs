using CwkSocial.APPLICATION.Models;
using CwkSocial.APPLICATION.UserProfiles.Commands;
using CwkSocial.DAL.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.APPLICATION.UserProfiles.CommandHandlers
{
    internal class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand, OperationResult<bool>>
    {
        private readonly DataContext _dataContext;

        public DeleteUserProfileCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<OperationResult<bool>> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<bool>();

            try
            {
                var userProfile = await _dataContext.UserProfiles
                .FirstOrDefaultAsync(userProfile => userProfile.UserProfileId == request.UserProfileId);

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

                _dataContext.UserProfiles.Remove(userProfile);
                await _dataContext.SaveChangesAsync();

                result.PayLoad = true;
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