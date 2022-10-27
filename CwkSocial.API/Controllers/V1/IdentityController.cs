using AutoMapper;
using CwkSocial.API.Contracts.Identity;
using CwkSocial.API.Filters;
using CwkSocial.APPLICATION.Identity.Commands;
using MediatR;
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
        public async Task<IActionResult> Register(UserRegistration userRegistration)
        {
            var command = _mapper.Map<RegisterCommand>(userRegistration);

            var response = await _mediator.Send(command);

            return response.IsError ? HandlerErrorResponse(response.Errors) : Ok(response);
        }
    }
}