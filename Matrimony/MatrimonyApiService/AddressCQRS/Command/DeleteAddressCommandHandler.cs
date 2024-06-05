using MatrimonyApiService.Commons;

namespace MatrimonyApiService.AddressCQRS.Command;

public class DeleteAddressCommandHandler : ICommandHandler<DeleteAddressCommand>
{
    private readonly IBaseRepo<Address.Address> _repository;
    private readonly IEventStore _eventStore;

    public DeleteAddressCommandHandler(IBaseRepo<Address.Address> repository, IEventStore eventStore)
    {
        _repository = repository;
        _eventStore = eventStore;
    }

    public async Task Handle(DeleteAddressCommand command)
    {
        var address = await _repository.GetById(command.Id);
        if (address == null) throw new KeyNotFoundException();

        await _repository.DeleteById(address.Id);

        var addressDeletedEvent = new AddressDeletedEvent
        {
            Id = address.Id
        };

        await _eventStore.SaveEventAsync(addressDeletedEvent);
    }
}
