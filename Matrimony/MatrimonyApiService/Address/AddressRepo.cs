using MatrimonyApiService.Commons;
using MatrimonyApiService.Repos;

namespace MatrimonyApiService.Address;

public class AddressRepo(MatrimonyContext context): BaseRepo<Address>(context);