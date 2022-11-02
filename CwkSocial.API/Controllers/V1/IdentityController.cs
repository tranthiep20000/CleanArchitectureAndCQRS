using AutoMapper;
using CwkSocial.API.Contracts.Identity;
using CwkSocial.API.Extensions;
using CwkSocial.API.Filters;
using CwkSocial.APPLICATION.Identity.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    [CwkSocialExceptionHandler]
    public class IdentityController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public IdentityController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [Route($"{ApiRoutes.Identity.Registration}")]
        [ValidateModel]
        public async Task<IActionResult> Register(UserRegistration userRegistration, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<RegisterCommand>(userRegistration);

            var response = await _mediator.Send(command, cancellationToken);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var authenticationResult = new AuthenticationResult() { Token = response.PayLoad };

            return Ok(authenticationResult);
        }

        [HttpPost]
        [Route($"{ApiRoutes.Identity.Login}")]
        [ValidateModel]
        public async Task<IActionResult> Login(Login login, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<LoginCommand>(login);

            var response = await _mediator.Send(command, cancellationToken);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var authenticationResult = new AuthenticationResult() { Token = response.PayLoad };

            return Ok(authenticationResult);
        }

        [HttpDelete]
        [Route($"{ApiRoutes.Identity.RemoveAccount}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RemoveAccount(CancellationToken cancellationToken)
        {
            var identityId = HttpContext.GetIdentityIdClaimValue();

            var command = new RemoveAccountCommand() { IdentityId = identityId };

            var response = await _mediator.Send(command, cancellationToken);

            return (response.IsError) ? HandlerErrorResponse(response.Errors) : Ok(response);
        }
    }
}