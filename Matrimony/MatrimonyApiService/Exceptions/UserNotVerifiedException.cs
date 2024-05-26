namespace MatrimonyApiService.Exceptions;

public class UserNotVerifiedException: Exception
{
    public UserNotVerifiedException()
    {
    }

    public UserNotVerifiedException(string? message) : base(message)
    {
    }
}