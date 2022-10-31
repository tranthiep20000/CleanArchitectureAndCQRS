namespace CwkSocial.APPLICATION.Models
{
    public class OperationResult<T>
    {
        public T PayLoad { get; set; }
        public bool IsError { get; private set; }
        public List<Error> Errors { get; } = new List<Error>();

        public void AddError(ErrorCode code, string message)
        {
            IsError = true;
            Errors.Add(new Error(){ Code = code, Message = $"{message}" });
        }

        public void ResetIsErrorFlag()
        {
            IsError = false;
        } 
    }
}