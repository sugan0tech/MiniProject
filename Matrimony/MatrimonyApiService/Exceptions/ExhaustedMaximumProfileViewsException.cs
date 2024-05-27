namespace MatrimonyApiService.Exceptions;

public class ExhaustedMaximumProfileViewsException: Exception
{
    public ExhaustedMaximumProfileViewsException()
    {
    }

    public ExhaustedMaximumProfileViewsException(string? message) : base(message)
    {
    }
}