using System.Diagnostics.CodeAnalysis;

namespace MatrimonyApiService.User;

[ExcludeFromCodeCoverage]
public record AuthReturnDto
{
    public string Token { get; init; }
}