namespace MatrimonyApiService.Auth;

public record PayloadDto
{
    public PayloadDto(int id, string email)
    {
        Id = id;
        Email = email;
    }

    public int Id { get; }
    public string Email { get; }

}