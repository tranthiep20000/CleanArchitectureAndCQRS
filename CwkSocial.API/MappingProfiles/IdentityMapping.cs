using AutoMapper;
using CwkSocial.API.Contracts.Identity;
using CwkSocial.APPLICATION.Identity.Commands;
using CwkSocial.APPLICATION.Identity.Dtos;

namespace CwkSocial.API.MappingProfiles
{
    public class IdentityMapping : Profile
    {
        public IdentityMapping()
        {
            CreateMap<UserRegistration, RegisterCommand>();
            CreateMap<Login, LoginCommand>();
            CreateMap<AuthenticationIdentityUserDto, AuthenticationIdentityUser>();
        }
    }
}