using CwkSocial.DOMAIN.Aggregates.PostAggregate;

namespace CwkSocial.API.Contracts.Posts.Requests
{
    public class PostInteractionCreateUpdate
    {
        public InteractionType InteractionType { get; set; }
    }
}
