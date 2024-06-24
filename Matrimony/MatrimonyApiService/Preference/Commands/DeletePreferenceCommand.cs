using MediatR;

namespace MatrimonyApiService.Preference.Commands;

public record DeletePreferenceCommand(int preferenceId) : IRequest<PreferenceDto>;