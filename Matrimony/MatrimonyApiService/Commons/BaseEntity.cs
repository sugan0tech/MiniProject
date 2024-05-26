using System.ComponentModel.DataAnnotations;

namespace MatrimonyApiService.Commons;

public class BaseEntity
{
    [Key] public int Id { get; set; }
}