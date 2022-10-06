namespace CwkSocial.DOMAIN.Aggregates.PostAggregate
{
    public class PostInteraction
    {
        private PostInteraction()
        {
        }

        public Guid InteractionId { get; private set; }
        public Guid PostId { get; private set; }
        public InteractionType InteractionType { get; private set; }

        // Factories
        public static PostInteraction CreatePostInteraction(Guid postId, InteractionType interactionType)
        {
            return new PostInteraction
            {
                PostId = postId,
                InteractionType = interactionType
            };
        }
    }
}