using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;

namespace CwkSocial.DOMAIN.Aggregates.PostAggregate
{
    public class Post
    {
        private readonly List<PostComment> _comments = new List<PostComment>();
        private readonly List<PostInteraction> _interactions = new List<PostInteraction>();

        private Post()
        {
        }

        public Guid PostId { get; private set; }
        public Guid UserProfileId { get; private set; }
        public UserProfile UserProfile { get; private set; }
        public string TextContent { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified { get; private set; }
        public IEnumerable<PostComment> Comments { get { return _comments; } }
        public IEnumerable<PostInteraction> Interactions { get { return _interactions; } }

        // Factories
        public static Post CreatePost(Guid userProfileId, string textContent)
        {
            return new Post
            {
                UserProfileId = userProfileId,
                TextContent = textContent,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
        }

        // Public method
        public void UpdatePostText(string textContent)
        {
            TextContent = textContent;
            LastModified = DateTime.UtcNow;
        }

        public void AddPostComment(PostComment postComment)
        {
            _comments.Add(postComment);
        }

        public void RemovePostComment(PostComment postComment)
        {
            _comments.Remove(postComment);
        }

        public void AddPostInteraction(PostInteraction postInteraction)
        {
            _interactions.Add(postInteraction);
        }

        public void RemovePostInteraction(PostInteraction postInteraction)
        {
            _interactions.Remove(postInteraction);
        }
    }
}