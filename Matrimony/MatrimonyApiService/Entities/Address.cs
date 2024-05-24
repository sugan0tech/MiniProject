using System.ComponentModel.DataAnnotations;

namespace MatrimonyApiService.Entities;

public class Address : BaseEntity
{
    // [Key] public int AddressId { get; set; }

    [MaxLength(35)] public string? Street { get; set; }
    [MaxLength(30)] public required string City { get; set; }
    [MaxLength(30)] public required string State { get; set; }
    [MaxLength(25)] public required string Country { get; set; }
}