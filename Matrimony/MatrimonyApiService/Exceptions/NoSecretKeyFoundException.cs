namespace MatrimonyApiService.Exceptions;

public class NoSecretKeyFoundException : Exception
{
    public NoSecretKeyFoundException(string? message) : base(message)
    {
    }
}