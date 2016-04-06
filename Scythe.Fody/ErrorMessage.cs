namespace Scythe.Fody
{
    public class ErrorMessage
    {
        public readonly string Message;
        public readonly ErrorType ErrorType;

        public ErrorMessage(string message, ErrorType errorType)
        {
            Message = message;
            ErrorType = errorType;
        }
    }
}