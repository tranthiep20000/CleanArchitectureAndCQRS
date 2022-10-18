using CwkSocial.DOMAIN.Exceptions;
using CwkSocial.DOMAIN.Validators.UserProfileValidators;

namespace CwkSocial.DOMAIN.Aggregates.UserProfileAggregate
{
    public class BasicInfo
    {
        private BasicInfo()
        {
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string CurrentCity { get; private set; }

        public static BasicInfo CreateBasicInfo(string firstName, string lastName, string emailAddress,
            string phoneNumber, DateTime dateOfBirth, string currentCity)
        {
            var objectToValidate =  new BasicInfo
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                PhoneNumber = phoneNumber,
                DateOfBirth = dateOfBirth,
                CurrentCity = currentCity
            };

            var validator = new BasicInfoValidator();
            var validationResult = validator.Validate(objectToValidate);

            if (validationResult.IsValid) return objectToValidate;

            var exception = new UserProfileValidateException("The user profile is not valid");

            foreach (var error in validationResult.Errors)
            {
                exception.ValidationErrors.Add(error.ErrorMessage);
            }

            throw exception;
        }
    }
}