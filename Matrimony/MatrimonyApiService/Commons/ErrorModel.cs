namespace MatrimonyApiService.Commons;

public record ErrorModel(int Status, string Message)
{
    public int Status { get; set; } = Status;
    public string Message { get; set; } = Message;
}