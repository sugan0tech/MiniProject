namespace MatrimonyApiService.Exceptions;

public class ExhaustedMaximumRequestsException: Exception
{
    public ExhaustedMaximumRequestsException()
    {
    }

    public ExhaustedMaximumRequestsException(string? message) : base(message)
    {
    }
}