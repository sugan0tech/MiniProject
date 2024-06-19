using MediatR;

namespace MatrimonyApiService.Profile;
public record DeleteProfileCommand(int profileId) : IRequest<ProfileDto>;
