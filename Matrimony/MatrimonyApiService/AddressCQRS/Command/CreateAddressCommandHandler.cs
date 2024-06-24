using MatrimonyApiService.AddressCQRS.Event;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.AddressCQRS.Command;

public class CreateAddressCommandHandler(IBaseRepo<Address.Address> repository, IEventStore eventStore)
    : ICommandHandler<CreateAddressCommand>
{
    public async Task Handle(CreateAddressCommand command)
    {
        var address = new Address.Address
        {
            UserId = command.UserId,
            Street = command.Street,
            City = command.City,
            State = command.State,
            Country = command.Country
        };

        await repository.Add(address);

        var addressCreatedEvent = new AddressCreatedEvent
        {
            Id = address.Id,
            Street = address.Street,
            City = address.City,
            State = address.State,
            Country = address.Country
        };

        await eventStore.SaveEventAsync(addressCreatedEvent);
    }
}