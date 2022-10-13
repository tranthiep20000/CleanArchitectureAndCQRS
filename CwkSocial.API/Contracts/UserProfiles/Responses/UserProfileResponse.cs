using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;

namespace CwkSocial.API.Contracts.UserProfiles.Responses
{
    public record UserProfileResponse
    {
        public Guid UserProfileId { get; set; }
        public BasicInfo BasicInfo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
    }
}