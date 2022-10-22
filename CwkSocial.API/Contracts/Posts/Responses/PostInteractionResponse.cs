using CwkSocial.DOMAIN.Aggregates.PostAggregate;

namespace CwkSocial.API.Contracts.Posts.Responses
{
    public class PostInteractionResponse
    {
        public Guid InteractionId { get; set; }
        public Guid PostId { get; set; }
        public InteractionType InteractionType { get; set; }
    }
}
