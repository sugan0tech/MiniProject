using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Address;

public class Address : BaseEntity
{
    [ForeignKey("userId")] [Required] public required int UserId { get; set; }
    public User.User? User { get; set; }
    [MaxLength(35)] public string? Street { get; set; }
    [MaxLength(30)] public required string City { get; set; }
    [MaxLength(30)] public required string State { get; set; }
    [MaxLength(25)] public required string Country { get; set; }
}