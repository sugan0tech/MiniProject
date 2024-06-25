using MediatR;

namespace MatrimonyApiService.User.Query;

public class GetUserQueryHandler(IUserService userService) : IRequestHandler<GetUserQuery, UserDto>
{
    public Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return userService.GetById(request.UserId);
    }
}