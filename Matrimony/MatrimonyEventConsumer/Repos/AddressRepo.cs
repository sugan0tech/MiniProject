using MatrimonyEventConsumer.Models;

namespace MatrimonyEventConsumer.Repos;

public class AddressRepo(ReplicaContext context) : BaseRepo<Address>(context);