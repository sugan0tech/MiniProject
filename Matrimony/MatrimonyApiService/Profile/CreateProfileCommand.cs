using MediatR;

namespace MatrimonyApiService.Profile;

public record CreateProfileCommand(ProfileDto ProfileDto) : IRequest<ProfileDto>;
