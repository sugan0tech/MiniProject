namespace MatrimonyApiService.Report;

public record ReportDto
{
    public int ReportId { get; set; }
    public int ProfileId { get; set; }
    public int ReportedById { get; set; }
    public DateTime ReportedAt { get; set; } = DateTime.Now;
}