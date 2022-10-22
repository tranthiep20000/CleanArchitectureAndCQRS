using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using FluentValidation;

namespace CwkSocial.DOMAIN.Validators.PostValidators
{
    public class PostInteractionValidator : AbstractValidator<PostInteraction>
    {
        public PostInteractionValidator()
        {
            RuleFor(postInteraction => postInteraction.InteractionType)
                .NotNull().WithMessage("Inreraction type should not be null")
                .NotEmpty().WithMessage("Inreraction type should not be empty");
        }
    }
}