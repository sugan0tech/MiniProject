namespace MatrimonyApiService.AddressCQRS;

public interface ICommandHandler<TCommand>
{
    Task Handle(TCommand command);
}
