using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Report;

public class ReportService(IBaseRepo<Report> repo, ILogger<BaseService<Report>> logger): BaseService<Report>(repo, logger);