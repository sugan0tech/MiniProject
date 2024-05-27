using AutoMapper;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Report;

public class ReportService(IBaseRepo<Report> repo, IMapper mapper, ILogger<BaseService<Report, ReportDto>> logger)
    : BaseService<Report, ReportDto>(repo, mapper, logger);