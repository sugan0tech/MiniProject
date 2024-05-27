using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Preference;
using MatrimonyApiService.Profile;
using MatrimonyApiService.ProfileView;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Profile;

public class ProfileServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.Profile.Profile>> _repoMock;
    private Mock<IPreferenceService> _preferenceServiceMock;
    private Mock<IProfileViewService> _profileViewServiceMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILogger<ProfileService>> _loggerMock;
    private ProfileService _profileService;

    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<IBaseRepo<MatrimonyApiService.Profile.Profile>>();
        _preferenceServiceMock = new Mock<IPreferenceService>();
        _profileViewServiceMock = new Mock<IProfileViewService>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<ProfileService>>();
        _profileService = new ProfileService(_repoMock.Object, _preferenceServiceMock.Object,
            _profileViewServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
        
        MatrimonyApiService.Profile.Profile profile = new MatrimonyApiService.Profile.Profile
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
        MatrimonyApiService.User.User user = new MatrimonyApiService.User.User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = "password"u8.ToArray(),
            HashKey = "key"u8.ToArray()
        };
        MatrimonyApiService.Membership.Membership membership = new MatrimonyApiService.Membership.Membership
        {
            Type = MemberShip.PremiumUser.ToString(), TypeEnum = MemberShip.PremiumUser, ProfileId = 1,
            Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false
        };
    }

    [Test]
    public async Task GetProfileById_ShouldReturnProfile_WhenProfileExists()
    {
        MatrimonyApiService.Profile.Profile profile = new MatrimonyApiService.Profile.Profile
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
        MatrimonyApiService.User.User user = new MatrimonyApiService.User.User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = "password"u8.ToArray(),
            HashKey = "key"u8.ToArray()
        };
        MatrimonyApiService.Membership.Membership membership = new MatrimonyApiService.Membership.Membership
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
        MatrimonyApiService.Profile.Profile profile = new MatrimonyApiService.Profile.Profile
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
        };        var preferenceDto = new PreferenceDto { PreferenceId = 1 };

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
        MatrimonyApiService.Profile.Profile profile = new MatrimonyApiService.Profile.Profile
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
        MatrimonyApiService.Profile.Profile profile = new MatrimonyApiService.Profile.Profile
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
        MatrimonyApiService.Profile.Profile profile = new MatrimonyApiService.Profile.Profile
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
        MatrimonyApiService.Profile.Profile profile = new MatrimonyApiService.Profile.Profile
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
        };        var profileDto = new ProfileDto { UserId = userId };

        _repoMock.Setup(r => r.GetAll()).ReturnsAsync(new List<MatrimonyApiService.Profile.Profile> { profile });
        _mapperMock.Setup(m => m.Map<ProfileDto>(profile)).Returns(profileDto);

        var result = await _profileService.GetProfileByUserId(userId);

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(userId, result.UserId);
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
            Preference = preferenceObj
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

        var result = await _profileService.GetMatches(profileId);

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(1, result.Count);
    }

    [Test]
    public async Task GetViews_ShouldReturnProfileViews()
    {
        MatrimonyApiService.Profile.Profile profile = new MatrimonyApiService.Profile.Profile
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
        MatrimonyApiService.User.User user = new MatrimonyApiService.User.User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = "password"u8.ToArray(),
            HashKey = "key"u8.ToArray()
        };
        MatrimonyApiService.Membership.Membership membership = new MatrimonyApiService.Membership.Membership
        {
            Id = 1, Type = MemberShip.PremiumUser.ToString(), ProfileId = 1,
            Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false
        };
        profile.Membership = membership;
        profile.MembershipId = 1;
 
        var profileId = 1;
        var profileViews = new List<ProfileViewDto>
        {
            new() { ViewedProfileAt = profileId, ViewedAt = DateTime.Now.AddMonths(-2) },
            new() { ViewedProfileAt = profileId, ViewedAt = DateTime.Now.AddMonths(-1) }
        };

        _repoMock.Setup(r => r.GetById(profileId)).ReturnsAsync(profile);
        _profileViewServiceMock.Setup(p => p.GetViewsByProfileId(profileId)).ReturnsAsync(profileViews);

        var result = await _profileService.GetViews(profileId);

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(2, result.Count);
    }

    [Test]
    public void GetViews_ShouldThrowNonPremiumUserException_WhenMembershipIsFree()
    {
        MatrimonyApiService.Profile.Profile profile = new MatrimonyApiService.Profile.Profile
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
        MatrimonyApiService.User.User user = new MatrimonyApiService.User.User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = "password"u8.ToArray(),
            HashKey = "key"u8.ToArray()
        };
        MatrimonyApiService.Membership.Membership membership = new MatrimonyApiService.Membership.Membership
        {
            Type = MemberShip.FreeUser.ToString(), ProfileId = 1,
            Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false
        };
        var profileId = 1;
        _repoMock.Setup(r => r.GetById(profileId)).ReturnsAsync(profile);

        Assert.ThrowsAsync<NonPremiumUserException>(() => _profileService.GetViews(profileId));
    }
}