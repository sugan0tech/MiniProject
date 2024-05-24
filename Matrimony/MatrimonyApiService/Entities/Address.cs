using System.ComponentModel.DataAnnotations;

namespace MatrimonyApiService.Entities;

public class Address : BaseEntity
{
    // [Key] public int AddressId { get; set; }
    public string street { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string country { get; set; }
}