using AutoMapper;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Preference;

public class PreferenceService(IBaseRepo<Preference> preferenceRepo, IMapper mapper): IPreferenceService
{

    /// <inheritdoc/>
    public async Task<PreferenceDto> Add(PreferenceDto preferenceDto)
    {
        try
        {
            var preferenceEntity = mapper.Map<Preference>(preferenceDto);
            var addedPreferenceEntity = await preferenceRepo.Add(preferenceEntity);
            return mapper.Map<PreferenceDto>(addedPreferenceEntity);
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding preference.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<PreferenceDto> GetById(int id)
    {
        try
        {
            var preferenceEntity = await preferenceRepo.GetById(id);
            return mapper.Map<PreferenceDto>(preferenceEntity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error getting preference with id {id}.", ex);
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
        catch (Exception ex)
        {
            throw new Exception("Error updating preference.", ex);
        }
    }
}