using CwkSocial.DOMAIN.Exceptions;
using CwkSocial.DOMAIN.Validators.PostValidators;

namespace CwkSocial.DOMAIN.Aggregates.PostAggregate
{
    public class PostComment
    {
        private PostComment()
        {
        }

        public Guid CommentId { get; private set; }
        public Guid PostId { get; private set; }
        public string Text { get; private set; }
        public Guid UserProfileId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified { get; private set; }

        // Factories
        /// <summary>
        /// Create a post comment
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <param name="text">Text content of post</param>
        /// <param name="userProfileId">Id of userprofile</param>
        /// <returns cref="PostComment"></returns>
        /// <exception cref="PostCommentValidateException"></exception>
        /// CreatedBy: ThiepTT(18/10/2022)
        public static PostComment CreatePostComment(Guid postId, string text, Guid userProfileId)
        {
            var objectToValidate =  new PostComment
            {
                PostId = postId,
                Text = text,
                UserProfileId = userProfileId,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            var validator = new PostCommentValidator();
            var validationResult = validator.Validate(objectToValidate);

            if (validationResult.IsValid) return objectToValidate;

            var exception = new PostCommentValidateException("Post comment is not valid");

            foreach(var error in validationResult.Errors)
            {
                exception.ValidationErrors.Add(error.ErrorMessage);
            }

            throw exception;
        }

        // Public method
        public void UpdateCommentText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                var exception = new PostValidateException("Cannot update post comment. Post comment text is not valid");
                exception.ValidationErrors.Add("The provided text is either null or contains only white space");

                throw exception;
            }

            Text = text;
            LastModified = DateTime.UtcNow;
        }
    }
}