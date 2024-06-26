namespace MatrimonyApiService.Exceptions;

public class InvalidProfileViewException: Exception
{
    public InvalidProfileViewException()
    {
    }

    public InvalidProfileViewException(string? message) : base(message)
    {
    }
}