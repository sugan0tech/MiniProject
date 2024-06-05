using MatrimonyApiService.Commons;

namespace MatrimonyApiService.AddressCQRS.Command;

public class CreateAddressCommandHandler : ICommandHandler<CreateAddressCommand>
{
    private readonly IBaseRepo<Address.Address> _repository;
    private readonly IEventStore _eventStore;

    public CreateAddressCommandHandler(IBaseRepo<Address.Address> repository, IEventStore eventStore)
    {
        _repository = repository;
        _eventStore = eventStore;
    }

    public async Task Handle(CreateAddressCommand command)
    {
        var address = new Address.Address
        {
            Street = command.Street,
            City = command.City,
            State = command.State,
            Country = command.Country
        };

        await _repository.Add(address);

        var addressCreatedEvent = new AddressCreatedEvent
        {
            Id = address.Id,
            Street = address.Street,
            City = address.City,
            State = address.State,
            Country = address.Country
        };

        await _eventStore.SaveEventAsync(addressCreatedEvent);
    }
}