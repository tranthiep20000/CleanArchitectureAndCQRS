using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using FluentValidation;

namespace CwkSocial.DOMAIN.Validators.PostValidators
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(post => post.TextContent)
                .NotNull().WithMessage("Post text content can't be null")
                .NotEmpty().WithMessage("Post text content can't be empty");
        }
    }
}