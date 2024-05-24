using System.ComponentModel.DataAnnotations;

namespace MatrimonyApiService.Entities;

public class BaseEntity
{
    [Key] public int Id { get; set; }
}