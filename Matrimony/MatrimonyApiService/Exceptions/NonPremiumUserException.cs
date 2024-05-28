namespace MatrimonyApiService.Exceptions;

public class NonPremiumUserException : Exception
{
    public NonPremiumUserException()
    {
    }

    public NonPremiumUserException(string? message) : base(message)
    {
    }
}