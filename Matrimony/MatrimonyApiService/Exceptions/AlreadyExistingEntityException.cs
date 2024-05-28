namespace MatrimonyApiService.Exceptions;

public class AlreadyExistingEntityException : Exception
{
    public AlreadyExistingEntityException()
    {
    }

    public AlreadyExistingEntityException(string? message) : base(message)
    {
    }
}