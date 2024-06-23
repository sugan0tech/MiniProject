using MatrimonyApiService.AddressCQRS.Event;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.AddressCQRS.Command;

public class UpdateAddressCommandHandler(IBaseRepo<Address.Address> repository, IEventStore eventStore)
    : ICommandHandler<UpdateAddressCommand>
{
    public async Task Handle(UpdateAddressCommand command)
    {
        var address = await repository.GetById(command.Id);
        if (address == null) throw new KeyNotFoundException();

        address.Street = command.Street;
        address.City = command.City;
        address.State = command.State;
        address.Country = command.Country;

        await repository.Update(address);

        var addressUpdatedEvent = new AddressUpdatedEvent
        {
            Id = address.Id,
            Street = address.Street,
            City = address.City,
            State = address.State,
            Country = address.Country
        };

        await eventStore.SaveEventAsync(addressUpdatedEvent);
    }
}
