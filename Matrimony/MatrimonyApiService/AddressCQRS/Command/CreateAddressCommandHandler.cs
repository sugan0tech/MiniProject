using MatrimonyApiService.AddressCQRS.Event;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.User.Commands;
using MatrimonyApiService.User.Query;
using MediatR;

namespace MatrimonyApiService.AddressCQRS.Command;

public class CreateAddressCommandHandler(IBaseRepo<Address> repository, IEventStore eventStore, EventProducerService eventProducer, IMediator mediator)
    : ICommandHandler<CreateAddressCommand>
{
    public async Task Handle(CreateAddressCommand command)
    {
        
        if (repository.GetAll().Result.Exists(address => address.UserId.Equals(command.UserId)))
        {
            throw new AlreadyExistingEntityException(
                "Unable to create new address, Address for the user already exists. consider updating it");
        }
        
        var user = await mediator.Send(new GetUserQuery(command.UserId));

        var address = new Address
        {
            UserId = command.UserId,
            Street = command.Street,
            City = command.City,
            State = command.State,
            Country = command.Country
        };

        var payload = new EventPayload
        {
            Address = address,
            EventType = "AddressCreatedEvent"
        };
        var @event = new AddressProducerEvent
        {
            AddressId = address.Id,
            EventPayload = payload
        };
        eventProducer.Produce(@event);
        
        await repository.Add(address);

        user.AddressId = address.Id;
        await mediator.Send(new UpdateUserCommand {userDto = user});

        var addressCreatedEvent = new AddressCreatedEvent
        {
            Id = address.Id,
            UserId = address.UserId,
            Street = address.Street,
            City = address.City,
            State = address.State,
            Country = address.Country
        };

        await eventStore.SaveEventAsync(addressCreatedEvent);
    }
}