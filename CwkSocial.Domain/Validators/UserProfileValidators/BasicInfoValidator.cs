using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using FluentValidation;

namespace CwkSocial.DOMAIN.Validators.UserProfileValidators
{
    public class BasicInfoValidator : AbstractValidator<BasicInfo>
    {
        public BasicInfoValidator()
        {
            RuleFor(basicInfo => basicInfo.FirstName)
                .NotNull().WithMessage("First name is required. It is currently null")
                .MinimumLength(3).WithMessage("First name must be at least 3 character long")
                .MaximumLength(50).WithMessage("First name can contain at most 50 character long");

            RuleFor(basicInfo => basicInfo.LastName)
                .NotNull().WithMessage("Last name is required. It is currently null")
                .MinimumLength(3).WithMessage("Last name must be at least 3 character long")
                .MaximumLength(50).WithMessage("Last name can contain at most 50 character long");

            RuleFor(basicInfo => basicInfo.EmailAddress)
                .NotNull().WithMessage("Email address is required. It is currently null")
                .EmailAddress().WithMessage("Provided string is not a correct email address format");

            RuleFor(basicInfo => basicInfo.DateOfBirth)
                .InclusiveBetween(new DateTime(DateTime.UtcNow.AddYears(-125).Ticks),
                    new DateTime(DateTime.UtcNow.AddYears(-18).Ticks))
                .WithMessage("Age need to be between 18 and 125");
        }
    }
}