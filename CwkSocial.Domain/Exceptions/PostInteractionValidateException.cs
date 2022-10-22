namespace CwkSocial.DOMAIN.Exceptions
{
    public class PostInteractionValidateException : NotValidateException
    {
        internal PostInteractionValidateException()
        {
        }

        internal PostInteractionValidateException(string message) : base(message)
        {
        }

        internal PostInteractionValidateException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}