using MediatR;

namespace MatrimonyApiService.Preference.Commands;

public class DeletePreferenceCommandHandler(IPreferenceService preferenceService): IRequestHandler<DeletePreferenceCommand, PreferenceDto>
{
    public Task<PreferenceDto> Handle(DeletePreferenceCommand request, CancellationToken cancellationToken)
    {
        return preferenceService.Delete(request.preferenceId);
    }
}