using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using CwkSocial.DOMAIN.Exceptions;
using CwkSocial.DOMAIN.Validators.PostValidators;

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
        /// <summary>
        /// Create a post
        /// </summary>
        /// <param name="userProfileId">Id of userprofile</param>
        /// <param name="textContent">Text content of post</param>
        /// <returns cref="Post"></returns>
        /// <exception cref="PostValidateException"></exception>
        /// CreatedBy: ThiepTT(18/10/2022)
        public static Post CreatePost(Guid userProfileId, string textContent)
        {
            var objectToValidate =  new Post
            {
                UserProfileId = userProfileId,
                TextContent = textContent,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            var validator = new PostValidator();
            var validationResult = validator.Validate(objectToValidate);

            if (validationResult.IsValid) return objectToValidate;

            var exception = new PostValidateException("Post is not valid");

            foreach (var error in validationResult.Errors)
            {
                exception.ValidationErrors.Add(error.ErrorMessage);
            }

            throw exception;
        }

        // Public method
        public void UpdatePostText(string textContent)
        {
            if (string.IsNullOrWhiteSpace(textContent))
            {
                var exception = new PostValidateException("Cannot update post. Post text is not valid");
                exception.ValidationErrors.Add("The provided text is either null or contains only white space");

                throw exception;
            }    

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