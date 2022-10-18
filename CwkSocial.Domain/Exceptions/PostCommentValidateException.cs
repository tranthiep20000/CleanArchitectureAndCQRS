namespace CwkSocial.DOMAIN.Exceptions
{
    public class PostCommentValidateException : NotValidateException
    {
        internal PostCommentValidateException()
        {
        }

        internal PostCommentValidateException(string message) : base(message)
        {
        }

        internal PostCommentValidateException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}