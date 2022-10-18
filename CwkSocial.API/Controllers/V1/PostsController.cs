using AutoMapper;
using CwkSocial.API.Contracts.Posts.Responses;
using CwkSocial.API.Filters;
using CwkSocial.APPLICATION.Posts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class PostsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PostsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var query = new GetAllPostsQuery();

            var response = await _mediator.Send(query);

            if(response.IsError)
                return HandlerErrorResponse(response.Errors);

            var posts = _mapper.Map<IEnumerable<PostResponse>>(response.PayLoad);

            return Ok(posts);
        }

        [HttpGet]
        [Route(ApiRoutes.Posts.IdRoute)]
        [ValidateGuid($"{ApiRoutes.Posts.IdRoute}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var query = new GetPostByIdQuery() { PostId = id };

            var response = await _mediator.Send(query);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var post = _mapper.Map<PostResponse>(response.PayLoad);

            return Ok(post);
        }
    }
}