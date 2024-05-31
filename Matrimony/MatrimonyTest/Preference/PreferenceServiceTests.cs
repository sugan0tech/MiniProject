using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Preference;
using MatrimonyApiService.Profile;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Preference;

public class PreferenceServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.Preference.Preference>> _mockRepo;
    private Mock<IProfileService> _mockProfileService;
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<PreferenceService>> _mockLogger;
    private PreferenceService _preferenceService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IBaseRepo<MatrimonyApiService.Preference.Preference>>();
        _mockProfileService = new Mock<IProfileService>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<PreferenceService>>();
        _preferenceService = new PreferenceService(_mockRepo.Object, _mockMapper.Object,
            _mockLogger.Object);
    }

    [Test]
    public async Task AddPreference_ValidPreferenceDto_ReturnsAddedPreferenceDto()
    {
        // Arrange
        var preferenceDto = new PreferenceDto
        {
            PreferenceId = 1, MotherTongue = "English", Religion = "Christian", Education = "Bachelor",
            Occupation = "Engineer", MinHeight = 150, MaxHeight = 180, MinAge = 25, MaxAge = 35,
            PreferenceForId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
        };
        var preference = new MatrimonyApiService.Preference.Preference
        {
            Id = 1, MotherTongue = "English", Religion = "Christian", Education = "Bachelor",
            Occupation = "Engineer", MinHeight = 150, MaxHeight = 180, MinAge = 25, MaxAge = 35,
            PreferenceForId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
        };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Preference.Preference>(preferenceDto))
            .Returns(preference);
        _mockRepo.Setup(repo => repo.Add(preference)).ReturnsAsync(preference);
        _mockMapper.Setup(mapper => mapper.Map<PreferenceDto>(preference)).Returns(preferenceDto);

        // Act
        var result = await _preferenceService.Add(preferenceDto);

        // ClassicAssert
        ClassicAssert.AreEqual(preferenceDto, result);
    }
    //
    // [Test]
    // public void AddPreference_ThrowsException_ReturnsException()
    // {
    //     // Arrange
    //     var preferenceDto = new PreferenceDto
    //     {
    //         PreferenceId = 1, MotherTongue = "English", Religion = "Christian", Education = "Bachelor",
    //         Occupation = "Engineer", MinHeight = 150, MaxHeight = 180, MinAge = 25, MaxAge = 35,
    //         PreferenceForId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    //     };
    //
    //     _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Preference.Preference>(preferenceDto))
    //         .Throws(new Exception("Error adding preference"));
    //
    //     // Act & ClassicAssert
    //     var ex = Assert.ThrowsAsync<Exception>(async () => await _preferenceService.Add(preferenceDto));
    //     ClassicAssert.AreEqual("Error adding preference.", ex.Message);
    // }

    [Test]
    public async Task GetPreferenceById_ValidId_ReturnsPreferenceDto()
    {
        // Arrange
        var preferenceId = 1;
        var preference = new MatrimonyApiService.Preference.Preference
        {
            Id = preferenceId, MotherTongue = "English", Religion = "Christian", Education = "Bachelor",
            Occupation = "Engineer", MinHeight = 150, MaxHeight = 180, MinAge = 25, MaxAge = 35,
            PreferenceForId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
        };
        var preferenceDto = new PreferenceDto
        {
            PreferenceId = preferenceId, MotherTongue = "English", Religion = "Christian", Education = "Bachelor",
            Occupation = "Engineer", MinHeight = 150, MaxHeight = 180, MinAge = 25, MaxAge = 35,
            PreferenceForId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
        };

        _mockRepo.Setup(repo => repo.GetById(preferenceId)).ReturnsAsync(preference);
        _mockMapper.Setup(mapper => mapper.Map<PreferenceDto>(preference)).Returns(preferenceDto);

        // Act
        var result = await _preferenceService.GetById(preferenceId);

        // ClassicAssert
        ClassicAssert.AreEqual(preferenceDto, result);
    }

    [Test]
    public void GetPreferenceById_InvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var preferenceId = 1;
        _mockRepo.Setup(repo => repo.GetById(preferenceId)).Throws(new KeyNotFoundException("Preference not found"));

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _preferenceService.GetById(preferenceId));
    }

    [Test]
    public async Task UpdatePreference_ValidPreferenceDto_ReturnsUpdatedPreferenceDto()
    {
        // Arrange
        var preferenceDto = new PreferenceDto
        {
            PreferenceId = 1, MotherTongue = "English", Religion = "Christian", Education = "Bachelor",
            Occupation = "Engineer", MinHeight = 150, MaxHeight = 180, MinAge = 25, MaxAge = 35,
            PreferenceForId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
        };
        var preference = new MatrimonyApiService.Preference.Preference
        {
            Id = 1, MotherTongue = "English", Religion = "Christian", Education = "Bachelor",
            Occupation = "Engineer", MinHeight = 150, MaxHeight = 180, MinAge = 25, MaxAge = 35,
            PreferenceForId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
        };

        _mockRepo.Setup(repo => repo.GetById(preferenceDto.PreferenceId)).ReturnsAsync(preference);
        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Preference.Preference>(preferenceDto))
            .Returns(preference);
        _mockRepo.Setup(repo => repo.Update(preference)).ReturnsAsync(preference);
        _mockMapper.Setup(mapper => mapper.Map<PreferenceDto>(preference)).Returns(preferenceDto);

        // Act
        var result = await _preferenceService.Update(preferenceDto);

        // Assert
        ClassicAssert.AreEqual(preferenceDto, result);
    }

    [Test]
    public void UpdatePreference_InvalidPreferenceDto_ThrowsKeyNotFoundException()
    {
        // Arrange
        var preferenceDto = new PreferenceDto
        {
            PreferenceId = 1, MotherTongue = "English", Religion = "Christian", Education = "Bachelor",
            Occupation = "Engineer", MinHeight = 150, MaxHeight = 180, MinAge = 25, MaxAge = 35,
            PreferenceForId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
        };
        var preference = new MatrimonyApiService.Preference.Preference
        {
            Id = 1, MotherTongue = "English", Religion = "Christian", Education = "Bachelor",
            Occupation = "Engineer", MinHeight = 150, MaxHeight = 180, MinAge = 25, MaxAge = 35,
            PreferenceForId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
        };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Preference.Preference>(preferenceDto))
            .Returns(preference);
        _mockRepo.Setup(repo => repo.Update(preference)).Throws(new KeyNotFoundException());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _preferenceService.Update(preferenceDto));
    }

    // [Test]
    // public async Task DeletePreferenceById_ValidId_ReturnsDeletedPreferenceDto()
    // {
    //     // Arrange
    //     var preferenceId = 1;
    //     var preference = new MatrimonyApiService.Preference.Preference
    //     {
    //         Id = preferenceId, MotherTongue = "English", Religion = "Christian", Education = "Bachelor",
    //         Occupation = "Engineer", MinHeight = 150, MaxHeight = 180, MinAge = 25, MaxAge = 35,
    //         PreferenceForId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    //     };
    //     var preferenceDto = new PreferenceDto
    //     {
    //         PreferenceId = preferenceId, MotherTongue = "English", Religion = "Christian", Education = "Bachelor",
    //         Occupation = "Engineer", MinHeight = 150, MaxHeight = 180, MinAge = 25, MaxAge = 35,
    //         PreferenceForId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    //     };
    //
    //     _mockRepo.Setup(repo => repo.DeleteById(preferenceId)).ReturnsAsync(preference);
    //     _mockMapper.Setup(mapper => mapper.Map<PreferenceDto>(preference)).Returns(preferenceDto);

    // Act
    // var result = await _preferenceService.Deleteb(preferenceId);
    //
    // // ClassicAssert
    // ClassicAssert.AreEqual(preferenceDto, result);
// }

// [Test]
// public void DeletePreferenceById_InvalidId_ThrowsKeyNotFoundException()
// {
//     // Arrange
//     var preferenceId = 1;
//     _mockRepo.Setup(repo => repo.DeleteById(preferenceId)).Throws(new KeyNotFoundException("Preference not found"));
//
//     // Act & ClassicAssert
//     ClassicAssert.ThrowsAsync<KeyNotFoundException>(async () => await _preferenceService.Delete(preferenceId));
// }

// [Test]
// public async Task GetAllPreferences_ReturnsListOfPreferenceDto()
// {
//     // Arrange
//     var preferences = new List<MatrimonyApiService.Preference.Preference>
//     {
//         new MatrimonyApiService.Preference.Preference
//         {
//             Id = 1, MotherTongue = "English", Religion = "Christian", Education = "Bachelor",
//             Occupation = "Engineer", MinHeight = 150, MaxHeight = 180, MinAge = 25, MaxAge = 35,
//             PreferenceForId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
//         },
//         new MatrimonyApiService.Preference.Preference
//         {
//             Id = 2, MotherTongue = "Hindi", Religion = "Hindu", Education = "Master",
//             Occupation = "Doctor", MinHeight = 160, MaxHeight = 190, MinAge = 28, MaxAge = 38,
//             PreferenceForId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
//         }
//     };
//     var preferenceDtos = new List<PreferenceDto>
//     {
//         new PreferenceDto
//         {
//             PreferenceId = 1, MotherTongue = "English", Religion = "Christian", Education = "Bachelor",
//             Occupation = "Engineer", MinHeight = 150, MaxHeight = 180, MinAge = 25, MaxAge = 35,
//             PreferenceForId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
//         },
//         new PreferenceDto
//         {
//             PreferenceId = 2, MotherTongue = "Hindi", Religion = "Hindu", Education = "Master",
//             Occupation = "Doctor", MinHeight = 160, MaxHeight = 190, MinAge = 28, MaxAge = 38,
//             PreferenceForId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
//         }
//     };
//
//     _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(preferences);
//     _mockMapper.Setup(mapper => mapper.Map<List<PreferenceDto>>(preferences)).Returns(preferenceDtos);
//
//     // Act
//     var result = await _preferenceService.GetAll();
//
//     // ClassicAssert
//     ClassicAssert.AreEqual(preferenceDtos, result);
// }
}