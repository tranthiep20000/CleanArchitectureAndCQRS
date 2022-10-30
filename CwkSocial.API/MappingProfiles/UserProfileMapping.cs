using AutoMapper;
using CwkSocial.API.Contracts.UserProfiles.Requests;
using CwkSocial.API.Contracts.UserProfiles.Responses;
using CwkSocial.APPLICATION.UserProfiles.Commands;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;

namespace CwkSocial.API.MappingProfiles
{
    public class UserProfileMapping : Profile
    {
        public UserProfileMapping()
        {
            CreateMap<UserProfileCreateUpdate, UpdateUserProfileBasicInfoCommand>();
            CreateMap<UserProfile, UserProfileResponse>();
            CreateMap<BasicInfo, BasicInfomation>();
        }
    }
}