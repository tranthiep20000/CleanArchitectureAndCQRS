namespace CwkSocial.DOMAIN.Aggregates.UserProfileAggregate
{
    public class UserProfile
    {
        private UserProfile()
        {
        }

        public Guid UserProfileId { get; private set; }
        public string IdentityId { get; private set; }
        public BasicInfo BasicInfo { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified { get; private set; }

        // Factories
        public static UserProfile CreateUserProfile(string indentityId, BasicInfo basicInfo)
        {
            // TODO: add validation, error handling strategies, error notification strategies

            return new UserProfile
            {
                IdentityId = indentityId,
                BasicInfo = basicInfo,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
        }

        // Public method
        public void UpdateBasicInfo(BasicInfo basicInfo)
        {
            BasicInfo = basicInfo;
            LastModified = DateTime.UtcNow;
        }
    }
}