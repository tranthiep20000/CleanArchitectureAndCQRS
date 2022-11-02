using CwkSocial.DOMAIN.Exceptions;
using CwkSocial.DOMAIN.Validators.PostValidators;

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
        public Guid UserProfileId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified { get; private set; }

        // Factories
        public static PostInteraction CreatePostInteraction(Guid postId, InteractionType interactionType, Guid userProfileId)
        {
            var objectToValidate = new PostInteraction
            {
                PostId = postId,
                InteractionType = interactionType,
                UserProfileId = userProfileId,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            var validator = new PostInteractionValidator();
            var validationResult = validator.Validate(objectToValidate);

            if (validationResult.IsValid) return objectToValidate;

            var exception = new PostInteractionValidateException("Post interaction is not valid");

            foreach (var error in validationResult.Errors)
            {
                exception.ValidationErrors.Add(error.ErrorMessage);
            }

            throw exception;
        }

        // Public method
        public void UpdateInteractionType(InteractionType type)
        {
            if (string.IsNullOrWhiteSpace(type.ToString()))
            {
                var exception = new PostValidateException("Cannot update post interaction. Post interaction type is not valid");
                exception.ValidationErrors.Add("The provided interaction is either null or contains only white space");

                throw exception;
            }

            InteractionType = type;
            LastModified = DateTime.UtcNow;
        }
    }
}