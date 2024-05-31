namespace MatrimonyApiService.Exceptions;

public class ExhaustedMaximumProfileViewsException : Exception
{

    public ExhaustedMaximumProfileViewsException(string? message) : base(message)
    {
    }
}