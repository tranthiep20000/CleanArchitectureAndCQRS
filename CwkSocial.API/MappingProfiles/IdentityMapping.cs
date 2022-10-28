using AutoMapper;
using CwkSocial.API.Contracts.Identity;
using CwkSocial.APPLICATION.Identity.Commands;

namespace CwkSocial.API.MappingProfiles
{
    public class IdentityMapping : Profile
    {
        public IdentityMapping()
        {
            CreateMap<UserRegistration, RegisterCommand>();
            CreateMap<Login, LoginCommand>();
        }
    }
}