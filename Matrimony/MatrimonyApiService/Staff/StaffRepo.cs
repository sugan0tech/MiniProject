using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Staff;

public class StaffRepo(MatrimonyContext context): BaseRepo<Staff>(context);