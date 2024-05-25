namespace MatrimonyApiService.ProfileView;

public record ProfileViewDto
{
    public int ViewerId { get; init; }
    public int ViewedProfileAt { get; init; }
    public DateTime ViewedAt { get; init; }
}