using System.Runtime.CompilerServices;
namespace SchoolSystem.Exceptions
{
    public class CustomException : Exception
    {
        public int StatusCode { get; }
        public string Title { get; }

        public string CallerMemberName { get; }
        public string CallerFilePath { get; }
        public int CallerLineNumber { get; }

        public CustomException(
            string message,
            string title,
            int statusCode,
            string callerMemberName,
            string callerFilePath,
            int callerLineNumber
        )
        : base(message)
        {
            StatusCode = statusCode;
            Title = title;
            CallerMemberName = callerMemberName;
            CallerFilePath = callerFilePath;
            CallerLineNumber = callerLineNumber;
        }
    }
    public class NotFoundException : CustomException
    {
        public NotFoundException(
            string message,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
        )
        : base(message, "Not Found", 404, callerMemberName, callerFilePath, callerLineNumber)
        {
        }
    }


    public class BadRequestException : CustomException
    {
        public BadRequestException(
            string message,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
        )
        : base(message, "Bad Request", 400, callerMemberName, callerFilePath, callerLineNumber)
        {
        }
    }

    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException(
            string message,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
        )
        : base(message, "Unauthorized", 401, callerMemberName, callerFilePath, callerLineNumber)
        {
        }
    }

}
