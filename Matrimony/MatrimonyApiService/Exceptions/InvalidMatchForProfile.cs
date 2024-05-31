namespace MatrimonyApiService.Exceptions;

public class InvalidMatchForProfile : Exception
{

    public InvalidMatchForProfile(string? message) : base(message)
    {
    }
}