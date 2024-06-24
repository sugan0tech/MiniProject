namespace MatrimonyApiService.Preference;

/// <summary>
/// Interface for managing user preferences.
/// </summary>
public interface IPreferenceService
{
    /// <summary>
    /// Adds a new preference asynchronously.
    /// </summary>
    Task<PreferenceDto> Add(PreferenceDto preferenceDto);

    /// <summary>
    /// Retrieves a preference by its unique identifier asynchronously.
    /// </summary>
    Task<PreferenceDto> GetById(int id);

    /// <summary>
    /// Retrieves a preference by its profile identifier asynchronously.
    /// </summary>
    Task<PreferenceDto> GetByProfileId(int id);

    /// <summary>
    /// Updates an existing preference asynchronously.
    /// </summary>
    Task<PreferenceDto> Update(PreferenceDto preferenceDto);

    /// <summary>
    /// Delete preference with the given Id.
    /// </summary>
    Task<PreferenceDto> Delete(int id);
}