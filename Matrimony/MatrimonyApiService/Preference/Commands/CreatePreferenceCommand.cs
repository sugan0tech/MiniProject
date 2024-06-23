using MediatR;

namespace MatrimonyApiService.Preference.Commands;

public record CreatePreferenceCommand(PreferenceDto PreferenceDto) : IRequest<PreferenceDto>;