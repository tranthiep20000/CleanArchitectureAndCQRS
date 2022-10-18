namespace CwkSocial.DOMAIN.Exceptions
{
    public class NotValidateException : Exception
    {
        internal NotValidateException()
        {
            ValidationErrors = new List<string>();
        }

        internal NotValidateException(string message) : base(message)
        {
            ValidationErrors = new List<string>();
        }

        internal NotValidateException(string message, Exception exception) : base(message, exception)
        {
            ValidationErrors = new List<string>();
        }

        public List<string> ValidationErrors { get; }
    }
}