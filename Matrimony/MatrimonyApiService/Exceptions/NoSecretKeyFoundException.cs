namespace MatrimonyApiService.Exceptions;

public class NoSecretKeyFoundException : Exception
{
    public NoSecretKeyFoundException()
    {
    }

    public NoSecretKeyFoundException(string? message) : base(message)
    {
    }
}