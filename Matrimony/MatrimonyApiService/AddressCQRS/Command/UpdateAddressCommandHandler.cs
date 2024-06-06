using MatrimonyApiService.AddressCQRS.Event;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.AddressCQRS.Command;

public class UpdateAddressCommandHandler : ICommandHandler<UpdateAddressCommand>
{
    private readonly IBaseRepo<Address.Address> _repository;
    private readonly IEventStore _eventStore;

    public UpdateAddressCommandHandler(IBaseRepo<Address.Address> repository, IEventStore eventStore)
    {
        _repository = repository;
        _eventStore = eventStore;
    }

    public async Task Handle(UpdateAddressCommand command)
    {
        var address = await _repository.GetById(command.Id);
        if (address == null) throw new KeyNotFoundException();

        address.Street = command.Street;
        address.City = command.City;
        address.State = command.State;
        address.Country = command.Country;

        await _repository.Update(address);

        var addressUpdatedEvent = new AddressUpdatedEvent
        {
            Id = address.Id,
            Street = address.Street,
            City = address.City,
            State = address.State,
            Country = address.Country
        };

        await _eventStore.SaveEventAsync(addressUpdatedEvent);
    }
}
