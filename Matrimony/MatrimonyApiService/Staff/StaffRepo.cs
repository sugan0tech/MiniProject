using MatrimonyApiService.Commons;
using MatrimonyApiService.Repos;

namespace MatrimonyApiService.Staff;

public class StaffRepo(MatrimonyContext context): BaseRepo<Staff>(context);