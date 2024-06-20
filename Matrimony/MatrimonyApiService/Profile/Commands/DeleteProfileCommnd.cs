using MediatR;

namespace MatrimonyApiService.Profile.Commands;
public record DeleteProfileCommand(int profileId) : IRequest<ProfileDto>;
