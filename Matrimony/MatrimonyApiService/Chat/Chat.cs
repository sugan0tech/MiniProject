using System.Diagnostics.CodeAnalysis;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Chat;

public class Chat : BaseEntity
{
    public string Name { get; set; } // to be uesed for groups

    [ExcludeFromCodeCoverage] public List<Profile.Profile> Participants { get; set; } = new();

    [ExcludeFromCodeCoverage] public List<Message.Message> Messages { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastMessageAt { get; set; }
}