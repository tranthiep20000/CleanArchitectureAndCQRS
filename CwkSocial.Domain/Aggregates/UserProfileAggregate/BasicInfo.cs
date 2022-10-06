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
            return new BasicInfo
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                PhoneNumber = phoneNumber,
                DateOfBirth = dateOfBirth,
                CurrentCity = currentCity
            };
        }
    }
}