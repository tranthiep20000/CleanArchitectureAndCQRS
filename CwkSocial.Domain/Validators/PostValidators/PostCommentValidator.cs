using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using FluentValidation;

namespace CwkSocial.DOMAIN.Validators.PostValidators
{
    public class PostCommentValidator : AbstractValidator<PostComment>
    {
        public PostCommentValidator()
        {
            RuleFor(postComment => postComment.Text)
                .NotNull().WithMessage("Comment text should not be null")
                .NotEmpty().WithMessage("Comment text should not be empty")
                .MaximumLength(1000)
                .MinimumLength(1);
        }
    }
}