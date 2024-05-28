namespace MatrimonyApiService.Exceptions;

public class InvalidMatchForProfile : Exception
{
    public InvalidMatchForProfile()
    {
    }

    public InvalidMatchForProfile(string? message) : base(message)
    {
    }
}