using MediatR;

namespace MatrimonyApiService.Profile.Commands;

public record CreateProfileCommand(ProfileDto ProfileDto) : IRequest<ProfileDto>;