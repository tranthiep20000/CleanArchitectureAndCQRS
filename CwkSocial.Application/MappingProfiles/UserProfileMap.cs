using AutoMapper;
using CwkSocial.APPLICATION.UserProfiles.Commands;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;

namespace CwkSocial.APPLICATION.MappingProfiles
{
    internal class UserProfileMap : Profile
    {
        public UserProfileMap()
        {
            CreateMap<CreateUserProfileCommand, BasicInfo>();
        }
    }
}