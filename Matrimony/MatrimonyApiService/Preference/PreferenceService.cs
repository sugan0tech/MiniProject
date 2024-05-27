using AutoMapper;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Preference;

public class PreferenceService(IBaseRepo<Preference> preferenceRepo, IMapper mapper, ILogger<PreferenceService> logger) : IPreferenceService
{
    /// <inheritdoc/>
    public async Task<PreferenceDto> Add(PreferenceDto preferenceDto)
    {
        var preferenceEntity = mapper.Map<Preference>(preferenceDto);
        var addedPreferenceEntity = await preferenceRepo.Add(preferenceEntity);
        return mapper.Map<PreferenceDto>(addedPreferenceEntity);
    }

    /// <inheritdoc/>
    public async Task<PreferenceDto> GetById(int id)
    {
        try
        {
            var preferenceEntity = await preferenceRepo.GetById(id);
            return mapper.Map<PreferenceDto>(preferenceEntity);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw ;
        }
    }

    /// <inheritdoc/>
    public async Task<PreferenceDto> Update(PreferenceDto preferenceDto)
    {
        try
        {
            var preferenceEntity = mapper.Map<Preference>(preferenceDto);
            var updatedPreferenceEntity = await preferenceRepo.Update(preferenceEntity);
            return mapper.Map<PreferenceDto>(updatedPreferenceEntity);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw ;
        }
    }
}