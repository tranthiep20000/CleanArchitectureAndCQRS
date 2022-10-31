namespace CwkSocial.APPLICATION.Identity
{
    public class IdentityErrorMessage
    {
        public const string IdentityUserDoesNotExsist = "Unable to find a user with the specified username";
        public const string IncorrectPassword = "The provided password is incorrect";
        public const string IdentityUserAlreadyExists = "Provided email address already exsits. Cannot register new user";
    }
}