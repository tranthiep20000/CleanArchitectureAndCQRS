using AutoMapper;
using CwkSocial.API.Contracts.Posts.Requests;
using CwkSocial.API.Contracts.Posts.Responses;
using CwkSocial.APPLICATION.Posts.Commands;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;

namespace CwkSocial.API.MappingProfiles
{
    public class PostMapping : Profile
    {
        public PostMapping()
        {
            CreateMap<Post, PostResponse>();
            CreateMap<PostCreate, CreatePostCommand>();
            CreateMap<PostComment, PostCommentResponse>();
            CreateMap<PostInteraction, PostInteractionResponse>();
        }
    }
}