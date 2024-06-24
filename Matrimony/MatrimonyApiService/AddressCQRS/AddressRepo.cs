using MatrimonyApiService.Commons;

namespace MatrimonyApiService.AddressCQRS;

public class AddressRepo(MatrimonyContext context) : BaseRepo<AddressCQRS.Address>(context);