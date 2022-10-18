using AutoMapper;
using CwkSocial.API.Contracts.Posts.Responses;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;

namespace CwkSocial.API.MappingProfiles
{
    public class PostMapping : Profile
    {
        public PostMapping()
        {
            CreateMap<Post, PostResponse>();
        }
    }
}