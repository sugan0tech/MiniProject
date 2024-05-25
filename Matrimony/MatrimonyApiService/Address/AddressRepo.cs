using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Address;

public class AddressRepo(MatrimonyContext context): BaseRepo<Address>(context);