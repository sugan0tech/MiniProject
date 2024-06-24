namespace MatrimonyApiService.AddressCQRS.Command;

public interface ICommandHandler<TCommand>
{
    Task Handle(TCommand command);
}