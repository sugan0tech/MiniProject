using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Report;

public class Report : BaseEntity
{
    [Required] [ForeignKey("ProfileId")] public required int ProfileId { get; set; }
    [Required] [ForeignKey("UserId")] public required int ReportedById { get; set; }
    public DateTime ReportedAt { get; set; } = DateTime.Now;
}