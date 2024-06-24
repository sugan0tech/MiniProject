namespace MatrimonyApiService.Exceptions;

public class MatchRequestToSelfException : Exception
{
    public MatchRequestToSelfException(string? message) : base(message)
    {
    }
}