namespace CwkSocial.DOMAIN.Exceptions
{
    public class PostValidateException : NotValidateException
    {
        internal PostValidateException()
        {
        }

        internal PostValidateException(string message) : base(message)
        {
        }

        internal PostValidateException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}