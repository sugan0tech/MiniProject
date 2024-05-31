namespace MatrimonyApiService.Exceptions;

public class UserNotVerifiedException : Exception
{
    public UserNotVerifiedException(string? message) : base(message)
    {
    }
}