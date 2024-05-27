namespace MatrimonyApiService.Exceptions;

public class InvalidDateTimeException: Exception
{
    public InvalidDateTimeException()
    {
    }

    public InvalidDateTimeException(string? message) : base(message)
    {
    }
}