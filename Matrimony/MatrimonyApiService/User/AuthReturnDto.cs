using System.Diagnostics.CodeAnalysis;

namespace MatrimonyApiService.User;

[ExcludeFromCodeCoverage]
public record AuthReturnDto
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}