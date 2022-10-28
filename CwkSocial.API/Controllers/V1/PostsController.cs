﻿using AutoMapper;
using CwkSocial.API.Contracts.Posts.Requests;
using CwkSocial.API.Contracts.Posts.Responses;
using CwkSocial.API.Filters;
using CwkSocial.APPLICATION.Posts.Commands;
using CwkSocial.APPLICATION.Posts.Queries;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var posts = _mapper.Map<IEnumerable<PostResponse>>(response.PayLoad);

            return Ok(posts);
        }

        [HttpGet]
        [Route($"{ApiRoutes.Posts.IdRoute}")]
        [ValidateGuid("id")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var query = new GetPostByIdQuery() { PostId = id };

            var response = await _mediator.Send(query);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var post = _mapper.Map<PostResponse>(response.PayLoad);

            return Ok(post);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreatePost([FromBody] PostCreate postCreate)
        {
            var command = _mapper.Map<CreatePostCommand>(postCreate);

            var response = await _mediator.Send(command);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var post = _mapper.Map<PostResponse>(response.PayLoad);

            return Ok(post);
        }

        [HttpPut]
        [Route($"{ApiRoutes.Posts.IdRoute}")]
        [ValidateGuid("id")]
        [ValidateModel]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] PostUpdate postUpdate)
        {
            var command = new UpdatePostCommand() { PostId = id, TextContent = postUpdate.TextContent };

            var response = await _mediator.Send(command);

            return response.IsError ? HandlerErrorResponse(response.Errors) : Ok(response);
        }

        [HttpDelete]
        [Route($"{ApiRoutes.Posts.IdRoute}")]
        [ValidateGuid("id")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var command = new DeletePostCommand() { PostId = id };

            var response = await _mediator.Send(command);

            return response.IsError ? HandlerErrorResponse(response.Errors) : Ok(response);
        }

        [HttpGet]
        [Route($"{ApiRoutes.Posts.PostComments}")]
        [ValidateGuid("postId")]
        public async Task<IActionResult> GetAllPostCommentsByPostId(Guid postId)
        {
            var query = new GetAllPostCommentsByPostIdQuery() { PostId = postId };

            var response = await _mediator.Send(query);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var postComments = _mapper.Map<IEnumerable<PostCommentResponse>>(response.PayLoad);

            return Ok(postComments);
        }

        [HttpGet]
        [Route($"{ApiRoutes.Posts.CommentById}")]
        [ValidateGuid("postId")]
        [ValidateGuid("commentId")]
        public async Task<IActionResult> GetPostCommentByIdToPost(Guid postId, Guid commentId)
        {
            var query = new GetPostCommentByIdToPostQuery() { PostId = postId, CommentId = commentId };

            var response = await _mediator.Send(query);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var postComment = _mapper.Map<PostCommentResponse>(response.PayLoad);

            return Ok(postComment);
        }

        [HttpPost]
        [Route($"{ApiRoutes.Posts.PostComments}")]
        [ValidateGuid("postId")]
        [ValidateModel]
        public async Task<IActionResult> AddPostCommentToPost(Guid postId, [FromBody] PostCommentCreate postComment)
        {
            var command = new AddPostCommentToPostCommand()
            {
                PostId = postId,
                UserProfileId = postComment.UserProfileId,
                TextComment = postComment.TextComment
            };

            var response = await _mediator.Send(command);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var postcomment = _mapper.Map<PostCommentResponse>(response.PayLoad);

            return Ok(postcomment);
        }

        [HttpPut]
        [Route($"{ApiRoutes.Posts.CommentById}")]
        [ValidateGuid("postId")]
        [ValidateGuid("commentId")]
        [ValidateModel]
        public async Task<IActionResult> UpdatePostCommentToPost(Guid postId, Guid commentId, [FromBody] PostCommentUpdate postComment)
        {
            return Ok();
        }

        [HttpDelete]
        [Route($"{ApiRoutes.Posts.CommentById}")]
        [ValidateGuid("postId")]
        [ValidateGuid("commentId")]
        public async Task<IActionResult> DeletePostCommentToPost(Guid postId, Guid commentId)
        {
            var command = new DeletePostCommentToPostCommand() { PostId = postId, CommentId = commentId };

            var response = await _mediator.Send(command);

            return (response.IsError) ? HandlerErrorResponse(response.Errors) : Ok(response);
        }

        [HttpGet]
        [Route($"{ApiRoutes.Posts.PostInteractions}")]
        [ValidateGuid("postId")]
        public async Task<IActionResult> GetAllPostInteractionsByPostId(Guid postId)
        {
            var query = new GetAllPostInteractionsByPostIdQuery() { PostId = postId };

            var response = await _mediator.Send(query);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var postInteractions = _mapper.Map<IEnumerable<PostInteractionResponse>>(response.PayLoad);

            return Ok(postInteractions);
        }

        [HttpGet]
        [Route($"{ApiRoutes.Posts.InteractionById}")]
        [ValidateGuid("postId")]
        [ValidateGuid("interactionId")]
        public async Task<IActionResult> GetPostInteractionByIdToPost(Guid postId, Guid interactionId)
        {
            var query = new GetPostInteractionByIdToPostQuery() { PostId = postId, InteractionId = interactionId };

            var response = await _mediator.Send(query);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var postInteraction = _mapper.Map<PostInteractionResponse>(response.PayLoad);

            return Ok(postInteraction);
        }

        [HttpPost]
        [Route($"{ApiRoutes.Posts.PostInteractions}")]
        [ValidateGuid("postId")]
        [ValidateModel]
        public  async Task<IActionResult> AddPostInteractionToPost(Guid postId, InteractionType interactionType)
        {
            var command = new AddPostInteractionToPostCommand() { PostId = postId, InteractionType = interactionType };

            var response = await _mediator.Send(command);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var postInteracion = _mapper.Map<PostInteractionResponse>(response.PayLoad);

            return Ok(postInteracion);
        }

        [HttpPut]
        [Route($"{ApiRoutes.Posts.InteractionById}")]
        [ValidateGuid("postId")]
        [ValidateGuid("interactionId")]
        [ValidateModel]
        public async Task<IActionResult> UpdatePostInteractionToPost(Guid postId, Guid interactionId, InteractionType interactionType)
        {
            return Ok();
        }

        [HttpDelete]
        [Route($"{ApiRoutes.Posts.InteractionById}")]
        [ValidateGuid("postId")]
        [ValidateGuid("interactionId")]
        public async Task<IActionResult> DeletePostInteractionToPost(Guid postId, Guid interactionId)
        {
            var command = new DeletePostInteractionToPostCommand() { PostId = postId, InteractionId = interactionId };

            var response = await _mediator.Send(command);

            return response.IsError ? HandlerErrorResponse(response.Errors) : Ok(response);
        }
    }
}