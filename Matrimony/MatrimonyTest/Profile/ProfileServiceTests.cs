using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Preference;
using MatrimonyApiService.Profile;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Profile;

public class ProfileServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.Profile.Profile>> _repoMock;
    private Mock<IPreferenceService> _preferenceServiceMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILogger<ProfileService>> _loggerMock;
    private ProfileService _profileService;

    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<IBaseRepo<MatrimonyApiService.Profile.Profile>>();
        _preferenceServiceMock = new Mock<IPreferenceService>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<ProfileService>>();
        _profileService = new ProfileService(_repoMock.Object, _preferenceServiceMock.Object,
             _mapperMock.Object, _loggerMock.Object);

        var profile = new MatrimonyApiService.Profile.Profile
        {
            Id = 1,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            RelationEnum = Relation.Friend,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        profile.EducationEnum = Education.NoEducation;
        profile.OccupationEnum = Occupation.Business;
        profile.MaritalStatusEnum = MaritalStatus.Single;
        profile.MotherTongueEnum = MotherTongue.English;
        profile.ReligionEnum = Religion.Christian;
        profile.EthnicityEnum = Ethnicity.Indian;
        profile.HabitEnum = Habit.Cooking;
        profile.GenderEnum = Gender.Female;
        var user = new MatrimonyApiService.User.User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = "password"u8.ToArray(),
            HashKey = "key"u8.ToArray()
        };
        var membership = new MatrimonyApiService.Membership.Membership
        {
            Type = MemberShip.PremiumUser.ToString(), TypeEnum = MemberShip.PremiumUser, ProfileId = 1,
            Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false
        };
    }

    [Test]
    public async Task GetProfileById_ShouldReturnProfile_WhenProfileExists()
    {
        var profile = new MatrimonyApiService.Profile.Profile
        {
            Id = 1,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            RelationEnum = Relation.Friend,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var user = new MatrimonyApiService.User.User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = "password"u8.ToArray(),
            HashKey = "key"u8.ToArray()
        };
        var membership = new MatrimonyApiService.Membership.Membership
        {
            Type = MemberShip.PremiumUser.ToString(), TypeEnum = MemberShip.PremiumUser, ProfileId = 1,
            Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false
        };
        var profileId = 1;
        var profileDto = new ProfileDto { ProfileId = profileId };

        _repoMock.Setup(r => r.GetById(profileId)).ReturnsAsync(profile);
        _mapperMock.Setup(m => m.Map<ProfileDto>(profile)).Returns(profileDto);

        var result = await _profileService.GetProfileById(profileId);

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(profileId, result.ProfileId);
    }
    
    [Test]
    public async Task GetProfilesByManager_ShouldReturnProfiles_WhenProfilesExist()
    {
        var managerId = 1;
        var profile = new MatrimonyApiService.Profile.Profile
        {
            Id = 1,
            ManagedById = managerId,
            DateOfBirth = new DateTime(1990,
                5,
                15),
            Education = "NoEducation",
            Occupation = "Engineer",
            MotherTongue = "English",
            Religion = "Christian",
            Height = 175,
            MaritalStatus = MaritalStatus.Single.ToString(),
            Ethnicity = Ethnicity.Indian.ToString(),
            Habit = Habit.Cooking.ToString(),
            Gender = Gender.Male.ToString(),
            ManagedByRelation = Relation.Self.ToString()
        };
        var profiles = new List<MatrimonyApiService.Profile.Profile> { profile };
        var profilePreviewDto = new ProfilePreviewDto { ProfileId = 1 , MaritalStatus = "Single"};

        _repoMock.Setup(r => r.GetAll()).ReturnsAsync(profiles);
        _mapperMock.Setup(m => m.Map<ProfilePreviewDto>(profile)).Returns(profilePreviewDto);

        var result = await _profileService.GetProfilesByManager(managerId);

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(1, result.Count);
        ClassicAssert.AreEqual(profilePreviewDto.ProfileId, result.First().ProfileId);
    }


    [Test]
    public void GetProfileById_ShouldThrowKeyNotFoundException_WhenProfileDoesNotExist()
    {
        var profileId = 1;

        _repoMock.Setup(r => r.GetById(profileId)).ThrowsAsync(new KeyNotFoundException());

        Assert.ThrowsAsync<KeyNotFoundException>(() => _profileService.GetProfileById(profileId));
    }

    [Test]
    public async Task AddProfile_ShouldAddProfileSuccessfully()
    {
        var profileDto = new ProfileDto
        {
            UserId = 1, MotherTongue = "English", Religion = "Christianity", Education = "Bachelors",
            Occupation = "Engineer", Height = 170, Age = 30
        };
        var profile = new MatrimonyApiService.Profile.Profile
        {
            Id = 1,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            RelationEnum = Relation.Friend,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var preferenceDto = new PreferenceDto { PreferenceId = 1 };

        _mapperMock.Setup(m => m.Map<MatrimonyApiService.Profile.Profile>(profileDto)).Returns(profile);
        _preferenceServiceMock.Setup(p => p.Add(It.IsAny<PreferenceDto>())).ReturnsAsync(preferenceDto);
        _repoMock.Setup(r => r.Add(profile)).ReturnsAsync(profile);
        _mapperMock.Setup(m => m.Map<ProfileDto>(profile)).Returns(profileDto);

        var result = await _profileService.AddProfile(profileDto);

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(profileDto.UserId, result.UserId);
        _preferenceServiceMock.Verify(p => p.Add(It.IsAny<PreferenceDto>()), Times.Once);
    }

    [Test]
    public async Task UpdateProfile_ShouldUpdateProfileSuccessfully()
    {
        var profileDto = new ProfileDto { ProfileId = 1, UserId = 1 };
        var profile = new MatrimonyApiService.Profile.Profile
        {
            Id = 1,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            RelationEnum = Relation.Friend,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        _mapperMock.Setup(m => m.Map<MatrimonyApiService.Profile.Profile>(profileDto)).Returns(profile);
        _repoMock.Setup(r => r.Update(profile)).ReturnsAsync(profile);
        _mapperMock.Setup(m => m.Map<ProfileDto>(profile)).Returns(profileDto);

        var result = await _profileService.UpdateProfile(profileDto);

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(profileDto.ProfileId, result.ProfileId);
    }

    [Test]
    public void UpdateProfile_ShouldThrowKeyNotFoundException_WhenProfileDoesNotExist()
    {
        var profileDto = new ProfileDto { ProfileId = 1 };
        var profile = new MatrimonyApiService.Profile.Profile
        {
            Id = 1,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            RelationEnum = Relation.Friend,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _mapperMock.Setup(m => m.Map<MatrimonyApiService.Profile.Profile>(profileDto))
            .Returns(profile);
        _repoMock.Setup(r => r.Update(It.IsAny<MatrimonyApiService.Profile.Profile>()))
            .ThrowsAsync(new KeyNotFoundException());

        Assert.ThrowsAsync<KeyNotFoundException>(() => _profileService.UpdateProfile(profileDto));
    }

    [Test]
    public async Task DeleteProfileById_ShouldDeleteProfileSuccessfully()
    {
        var profileId = 1;
        var profile = new MatrimonyApiService.Profile.Profile
        {
            Id = 1,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            RelationEnum = Relation.Friend,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        _repoMock.Setup(r => r.DeleteById(profileId)).ReturnsAsync(profile);
        _mapperMock.Setup(m => m.Map<ProfileDto>(profile)).Returns(new ProfileDto { ProfileId = profileId });

        var result = await _profileService.DeleteProfileById(profileId);

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(profileId, result.ProfileId);
    }

    [Test]
    public void DeleteProfileById_ShouldThrowKeyNotFoundException_WhenProfileDoesNotExist()
    {
        var profileId = 1;

        _repoMock.Setup(r => r.DeleteById(profileId)).ThrowsAsync(new KeyNotFoundException());

        Assert.ThrowsAsync<KeyNotFoundException>(() => _profileService.DeleteProfileById(profileId));
    }

    [Test]
    public async Task GetProfileByUserId_ShouldReturnProfile_WhenProfileExists()
    {
        var userId = 1;
        var profile = new MatrimonyApiService.Profile.Profile
        {
            Id = 1,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            RelationEnum = Relation.Friend,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var profileDto = new ProfileDto { UserId = userId };

        _repoMock.Setup(r => r.GetAll()).ReturnsAsync(new List<MatrimonyApiService.Profile.Profile> { profile });
        _mapperMock.Setup(m => m.Map<ProfileDto>(profile)).Returns(profileDto);

        var result = await _profileService.GetProfileByUserId(userId);

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(userId, result.UserId);
    }

    [Test]
    public async Task GetProfilePreviewById_ShouldReturnProfilePreview_WhenProfileExists()
    {
        var profileId = 1;
        var profile = new MatrimonyApiService.Profile.Profile
        {
            Id = profileId,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            Occupation = "Engineer",
            MotherTongue = "English",
            Religion = "Christian",
            Height = 175,
            MaritalStatus = MaritalStatus.Single.ToString(),
            Ethnicity = Ethnicity.Indian.ToString(),
            Habit = Habit.Cooking.ToString(),
            Gender = Gender.Male.ToString(),
            ManagedByRelation = Relation.Self.ToString()
        };
        var profilePreviewDto = new ProfilePreviewDto { ProfileId = profileId };

        _repoMock.Setup(r => r.GetById(profileId)).ReturnsAsync(profile);
        _mapperMock.Setup(m => m.Map<ProfilePreviewDto>(profile)).Returns(profilePreviewDto);

        var result = await _profileService.GetProfilePreviewById(profileId);

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(profilePreviewDto.ProfileId, result.ProfileId);
    }

    [Test]
    public void GetProfilePreviewById_ShouldThrowKeyNotFoundException_WhenProfileDoesNotExist()
    {
        var profileId = 1;

        _repoMock.Setup(r => r.GetById(profileId)).ThrowsAsync(new KeyNotFoundException());

        Assert.ThrowsAsync<KeyNotFoundException>(() => _profileService.GetProfilePreviewById(profileId));
    }

    [Test]
    public async Task GetMatches_ShouldReturnMatchedProfiles()
    {
        var profileId = 1;
        var preferenceId = 1;
        var preferenceObj = new MatrimonyApiService.Preference.Preference
        {
            Id = 1,
            MotherTongue = "English",
            Religion = Religion.Christian.ToString(),
            Education = Education.NoEducation.ToString(),
            Occupation = "Engineer",
            MinHeight = 165,
            MaxHeight = 176,
            MinAge = 25,
            MaxAge = 35
        };
        var profile1 = new MatrimonyApiService.Profile.Profile
        {
            Id = 1,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = Habit.PetsLover.ToString(),
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            RelationEnum = Relation.Friend,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            PreferenceId = 1,
            Preference = preferenceObj
        };
        var profile2 = new MatrimonyApiService.Profile.Profile
        {
            Id = 2,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = profile1.OccupationEnum.ToString(),
            MaritalStatus = profile1.MaritalStatusEnum.ToString(),
            MotherTongue = profile1.MotherTongueEnum.ToString(),
            Religion = profile1.ReligionEnum.ToString(),
            Ethnicity = profile1.EthnicityEnum.ToString(),
            Habit = profile1.HabitEnum.ToString(),
            Gender = Gender.Female.ToString(),
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            RelationEnum = Relation.Friend,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var preference = new PreferenceDto
        {
            MotherTongue = "English",
            Religion = Religion.Christian.ToString(),
            Education = Education.NoEducation.ToString(),
            Occupation = "Engineer",
            MinHeight = 165,
            MaxHeight = 176,
            MinAge = 25,
            MaxAge = 35
        };

        var profiles = new List<MatrimonyApiService.Profile.Profile>
        {
            profile1,
            profile2
        };

        _repoMock.Setup(r => r.GetById(profileId)).ReturnsAsync(profile1);
        _preferenceServiceMock.Setup(p => p.GetById(preferenceId)).ReturnsAsync(preference);
        _repoMock.Setup(r => r.GetAll()).ReturnsAsync(profiles);
        _mapperMock.Setup(m => m.Map<ProfilePreviewDto>(It.IsAny<MatrimonyApiService.Profile.Profile>()))
            .Returns(new ProfilePreviewDto());

        var result = await _profileService.GetProfileMatches(profileId);

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(1, result.Count);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllProfiles()
    {
        var preference = new MatrimonyApiService.Preference.Preference
        {
            MotherTongue = "English",
            Religion = Religion.Christian.ToString(),
            Education = Education.NoEducation.ToString(),
            Occupation = "Engineer",
            MinHeight = 165,
            MaxHeight = 176,
            MinAge = 25,
            MaxAge = 35
        };
        var profile1 = new MatrimonyApiService.Profile.Profile
        {
            Id = 1,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            RelationEnum = Relation.Friend,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            PreferenceId = 1,
            Preference = preference
        };
        var profile2 = new MatrimonyApiService.Profile.Profile
        {
            Id = 2,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = Religion.Christian.ToString(),
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            RelationEnum = Relation.Friend,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var profiles = new List<MatrimonyApiService.Profile.Profile>
        {
            profile1,
            profile2
        };
        var profileDtos = profiles.Select(p => new ProfileDto { ProfileId = p.Id }).ToList();

        _repoMock.Setup(r => r.GetAll()).ReturnsAsync(profiles);
        _mapperMock.Setup(m => m.Map<ProfileDto>(It.IsAny<MatrimonyApiService.Profile.Profile>()))
            .Returns((MatrimonyApiService.Profile.Profile p) => profileDtos.First(d => d.ProfileId == p.Id));

        var result = await _profileService.GetAll();

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(profileDtos.Count, result.Count);
        ClassicAssert.AreEqual(profileDtos.First().ProfileId, result.First().ProfileId);
    }


    [Test]
    public void GetProfileMatches_InvalidProfile_ThrowsKeyNotFoundException()
    {
        // Arrange
        var profileId = 1;
        _repoMock.Setup(repo => repo.GetById(1)).ThrowsAsync(new KeyNotFoundException());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _profileService.GetProfileMatches(profileId));
    }
}