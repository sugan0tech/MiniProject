using MatrimonyApiService.AddressCQRS.Event;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.AddressCQRS.Command;

public class DeleteAddressCommandHandler(IBaseRepo<Address.Address> repository, IEventStore eventStore)
    : ICommandHandler<DeleteAddressCommand>
{
    public async Task Handle(DeleteAddressCommand command)
    {
        var address = await repository.GetById(command.Id);
        if (address == null) throw new KeyNotFoundException();

        await repository.DeleteById(address.Id);

        var addressDeletedEvent = new AddressDeletedEvent
        {
            Id = address.Id
        };

        await eventStore.SaveEventAsync(addressDeletedEvent);
    }
}
