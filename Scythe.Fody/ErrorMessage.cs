namespace Scythe.Fody
{
    using System;

    public class ErrorMessage
    {
        public readonly string Message;

        public readonly ErrorType ErrorType;

        public readonly Severity Severity;

        public ErrorMessage(string message, ErrorType errorType, string severity)
        {
            Message = message;
            ErrorType = errorType;
            Severity = (Severity)Enum.Parse(typeof(Severity), severity);
        }
    }
}