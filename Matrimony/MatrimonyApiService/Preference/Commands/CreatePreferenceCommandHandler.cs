using MediatR;

namespace MatrimonyApiService.Preference.Commands;

public class CreatePreferenceCommandHandler(IPreferenceService preferenceService): IRequestHandler<CreatePreferenceCommand, PreferenceDto>
{
    public Task<PreferenceDto> Handle(CreatePreferenceCommand request, CancellationToken cancellationToken)
    {
        preferenceService.Add(request.PreferenceDto);
        throw new NotImplementedException();
    }
}