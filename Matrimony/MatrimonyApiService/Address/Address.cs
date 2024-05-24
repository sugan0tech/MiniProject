using System.ComponentModel.DataAnnotations;
using MatrimonyApiService.Entities;

namespace MatrimonyApiService.Address;

public class Address : BaseEntity
{
    // [Key] public int AddressId { get; set; }

    [MaxLength(35)] public string? Street { get; set; }
    [MaxLength(30)] public required string City { get; set; }
    [MaxLength(30)] public required string State { get; set; }
    [MaxLength(25)] public required string Country { get; set; }
}