using CwkSocial.DOMAIN.Aggregates.PostAggregate;

namespace CwkSocial.API.Contracts.Posts.Responses
{
    public class PostInteractionResponse
    {
        public Guid InteractionId { get; set; }
        public Guid PostId { get; set; }
        public string InteractionType { get; set; }
        public Guid UserProfileId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
    }
}
