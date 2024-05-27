using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Report;

public class ReportRepo(MatrimonyContext context) : BaseRepo<Report>(context);