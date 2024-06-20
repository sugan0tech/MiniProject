using AutoMapper;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Preference;

public class PreferenceService(IBaseRepo<Preference> preferenceRepo, IMapper mapper, ILogger<PreferenceService> logger)
    : IPreferenceService
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
            throw;
        }
    }
    
    /// <inheritdoc/>
    public async Task<PreferenceDto> GetByProfileId(int profileId)
    {
        try
        {
            var entities = await preferenceRepo.GetAll();
            return mapper.Map<PreferenceDto>(entities.Find(p => p.PreferenceForId.Equals(profileId)));
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
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
            throw;
        }
    }
    
    /// <inheritdoc/>
    public async Task<PreferenceDto> Delete(int id)
    {
        try
        {
            var preference = await preferenceRepo.DeleteById(id);
            return mapper.Map<PreferenceDto>(preference);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }
}