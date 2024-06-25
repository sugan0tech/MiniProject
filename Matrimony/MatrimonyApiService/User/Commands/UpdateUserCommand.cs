using MediatR;

namespace MatrimonyApiService.User.Commands;

public class UpdateUserCommand : IRequest<UserDto>
{
    public UserDto userDto { get; set; }
}