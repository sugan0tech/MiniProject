using MediatR;

namespace MatrimonyApiService.User.Commands;

public class UpdateUserCommandHandler(IUserService userService, IMediator mediator, ILogger<UpdateUserCommandHandler> logger): IRequestHandler<UpdateUserCommand, UserDto>
{
    public Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        return userService.Update(request.userDto);
    }
}