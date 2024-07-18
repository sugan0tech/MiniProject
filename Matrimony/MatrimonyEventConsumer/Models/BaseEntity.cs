using System.ComponentModel.DataAnnotations;

namespace MatrimonyEventConsumer.Models;

public class BaseEntity
{
    [Key] public int Id { get; set; }
}