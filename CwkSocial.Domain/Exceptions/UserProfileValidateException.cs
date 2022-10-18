namespace CwkSocial.DOMAIN.Exceptions
{
    public class UserProfileValidateException : NotValidateException
    {
        internal UserProfileValidateException()
        {
        }

        internal UserProfileValidateException(string message) : base(message)
        {
        }

        internal UserProfileValidateException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}