using MatrimonyApiService.Commons;
using MatrimonyApiService.Profile;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;
using System.ComponentModel.DataAnnotations;
using MatrimonyApiService.Commons.Enums;

namespace MatrimonyTest.Profile;

[TestFixture]
public class ProfileRepoTests
{
    private DbContextOptions<MatrimonyContext> _dbContextOptions;
    private MatrimonyContext _context;
    private ProfileRepo _profileRepo;

    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<MatrimonyContext>()
            .UseInMemoryDatabase("MatrimonyTestDb")
            .Options;

        _context = new MatrimonyContext(_dbContextOptions);
        _profileRepo = new ProfileRepo(_context);

        // PreSeeds
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
        _context.Users.AddAsync(user);
        _context.SaveChangesAsync();
        var preference = new MatrimonyApiService.Preference.Preference
        {
            MotherTongue = MotherTongue.English.ToString(),
            MotherTongueEnum = MotherTongue.English,
            Religion = Religion.Christian.ToString(),
            ReligionEnum = Religion.Christian,
            Education = Education.NoEducation.ToString(),
            EducationEnum = Education.NoEducation,
            Occupation = Occupation.Engineer.ToString(),
            MinHeight = 160,
            MaxHeight = 180,
            MinAge = 25,
            MaxAge = 35,
            PreferenceForId = 1
        };
        _context.Preferences.AddAsync(preference);
        _context.SaveChangesAsync();
        var membership = new MatrimonyApiService.Membership.Membership
        {
            Type = MemberShip.PremiumUser.ToString(), TypeEnum = MemberShip.PremiumUser, ProfileId = 1,
            Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false
        };
        _context.Memberships.AddAsync(membership);
        _context.SaveChangesAsync();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetById_ShouldReturnEntity_WhenEntityExists()
    {
        // Arrange
        var profile = new MatrimonyApiService.Profile.Profile
        {
            DateOfBirth = new DateTime(1990, 1, 1),
            Education = Education.NoEducation.ToString(),
            Occupation = Occupation.Engineer.ToString(),
            MaritalStatus = MaritalStatus.Single.ToString(),
            MotherTongue = MotherTongue.English.ToString(),
            Religion = Religion.Christian.ToString(),
            Ethnicity = Ethnicity.Indian.ToString(),
            Gender = Gender.Male.ToString(),
            ManagedByRelation = Relation.Self.ToString(),
            MembershipId = 1,
            AnnualIncome = 100,
            Habit = "None",
            UserId = 1,
            PreferenceId = 1,
            ManagedById = 1,
            Height = 1,
            Weight = 2
        };

        await _context.Profiles.AddAsync(profile);
        await _context.SaveChangesAsync();

        // Act
        var result = await _profileRepo.GetById(profile.Id);

        // Assert
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(Education.NoEducation, result.EducationEnum);
        ClassicAssert.AreEqual(Occupation.Engineer, result.OccupationEnum);
    }

    [Test]
    public void GetById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _profileRepo.GetById(99));
        ClassicAssert.AreEqual("Profile with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        var user2 = new MatrimonyApiService.User.User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = "password"u8.ToArray(),
            HashKey = "key"u8.ToArray()
        };
        await _context.Users.AddAsync(user2);
        await _context.SaveChangesAsync();
        var preference2 = new MatrimonyApiService.Preference.Preference
        {
            MotherTongue = MotherTongue.English.ToString(),
            MotherTongueEnum = MotherTongue.English,
            Religion = Religion.Christian.ToString(),
            ReligionEnum = Religion.Christian,
            Education = Education.NoEducation.ToString(),
            EducationEnum = Education.NoEducation,
            Occupation = Occupation.Engineer.ToString(),
            MinHeight = 160,
            MaxHeight = 180,
            MinAge = 25,
            MaxAge = 35,
            PreferenceForId = 2
        };
        await _context.Preferences.AddAsync(preference2);
        await _context.SaveChangesAsync();
        var membership2 = new MatrimonyApiService.Membership.Membership
        {
            Type = MemberShip.PremiumUser.ToString(), TypeEnum = MemberShip.PremiumUser, ProfileId = 2,
            Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false
        };
        await _context.Memberships.AddAsync(membership2);
        await _context.SaveChangesAsync();
        await _context.Profiles.AddRangeAsync(
            new MatrimonyApiService.Profile.Profile
            {
                DateOfBirth = new DateTime(1990, 1, 1),
                Education = "Graduate",
                Occupation = "Engineer",
                MaritalStatus = "Single",
                MotherTongue = "English",
                Religion = "Christian",
                Ethnicity = "Asian",
                Gender = Gender.Male.ToString(),
                ManagedByRelation = Relation.Self.ToString(),
                MembershipId = 1,
                AnnualIncome = 100,
                Habit = "None",
                UserId = 1,
                PreferenceId = 1,
                ManagedById = 1,
                Height = 1,
                Weight = 2
            },
            new MatrimonyApiService.Profile.Profile
            {
                DateOfBirth = new DateTime(1985, 5, 5),
                Education = "PostGraduate",
                Occupation = "Doctor",
                MaritalStatus = "Divorced",
                MotherTongue = "Hindi",
                Religion = "Hindu",
                Ethnicity = "Asian",
                Gender = Gender.Male.ToString(),
                ManagedByRelation = Relation.Self.ToString(),
                MembershipId = 2,
                AnnualIncome = 100,
                Habit = "None",
                UserId = 2,
                PreferenceId = 2,
                ManagedById = 2,
                Height = 1,
                Weight = 2
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _profileRepo.GetAll();

        // Assert
        ClassicAssert.AreEqual(2, result.Count);
    }

    [Test]
    public async Task Add_ShouldAddEntity()
    {
        // Arrange
        var profile = new MatrimonyApiService.Profile.Profile
        {
            DateOfBirth = new DateTime(1990, 1, 1),
            Education = "Graduate",
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Asian",
            Gender = Gender.Male.ToString(),
            ManagedByRelation = Relation.Self.ToString(),
            MembershipId = 1,
            AnnualIncome = 100,
            Habit = "None",
            UserId = 1,
            PreferenceId = 1,
            ManagedById = 1,
            Height = 1,
            Weight = 2
        };

        // Act
        var result = await _profileRepo.Add(profile);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual("Graduate", result.Education);
        ClassicAssert.AreEqual(1, await _context.Profiles.CountAsync());
    }

    [Test]
    public void Add_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _profileRepo.Add(null));
        ClassicAssert.AreEqual("Profile cannot be null. (Parameter 'entity')", ex.Message);
    }

    [Test]
    public async Task Update_ShouldUpdateEntity()
    {
        // Arrange
        var profile = new MatrimonyApiService.Profile.Profile
        {
            DateOfBirth = new DateTime(1990, 1, 1),
            Education = "Graduate",
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Asian",
            Gender = Gender.Male.ToString(),
            ManagedByRelation = Relation.Self.ToString(),
            MembershipId = 1,
            AnnualIncome = 100,
            Habit = "None",
            UserId = 1,
            PreferenceId = 1,
            ManagedById = 1,
            Height = 1,
            Weight = 2
        };
        await _context.Profiles.AddAsync(profile);
        await _context.SaveChangesAsync();

        profile.Education = "PostGraduate";

        // Act
        var result = await _profileRepo.Update(profile);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual("PostGraduate", result.Education);
    }

    [Test]
    public void Update_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Arrange
        var updateProfile = new MatrimonyApiService.Profile.Profile
        {
            Id = 99,
            DateOfBirth = new DateTime(1990, 1, 1),
            Education = "Graduate",
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Asian",
            Gender = Gender.Male.ToString(),
            ManagedByRelation = Relation.Self.ToString(),
            MembershipId = 1,
            AnnualIncome = 100,
            Habit = "None",
            UserId = 1,
            PreferenceId = 1,
            ManagedById = 1,
            Height = 1,
            Weight = 2
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _profileRepo.Update(updateProfile));
        ClassicAssert.AreEqual("Profile with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task DeleteById_ShouldDeleteEntity()
    {
        // Arrange
        var profile = new MatrimonyApiService.Profile.Profile
        {
            DateOfBirth = new DateTime(1990, 1, 1),
            Education = "Graduate",
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Asian",
            Gender = Gender.Male.ToString(),
            ManagedByRelation = Relation.Self.ToString(),
            MembershipId = 1,
            AnnualIncome = 100,
            Habit = "None",
            UserId = 1,
            PreferenceId = 1,
            ManagedById = 1,
            Height = 1,
            Weight = 2
        };
        await _context.Profiles.AddAsync(profile);
        await _context.SaveChangesAsync();

        // Act
        await _profileRepo.DeleteById(profile.Id);

        // Assert
        ClassicAssert.AreEqual(0, await _context.Profiles.CountAsync());
    }

    [Test]
    public void DeleteById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _profileRepo.DeleteById(99));
        ClassicAssert.AreEqual("Profile with key 99 not found!!!", ex.Message);
    }

    [Test]
    public void Profile_ShouldFailValidation_WhenInvalidEducationIsProvided()
    {
        // Arrange
        var profile = new MatrimonyApiService.Profile.Profile
        {
            DateOfBirth = new DateTime(1990, 1, 1),
            Education = "InvalidEducation",
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Asian",
            Gender = Gender.Male.ToString(),
            ManagedByRelation = Relation.Self.ToString(),
            MembershipId = 1,
            AnnualIncome = 100,
            Habit = "None",
            UserId = 1,
            PreferenceId = 1,
            ManagedById = 1,
            Height = 1,
            Weight = 2
        };

        var context = new ValidationContext(profile, null, null);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(profile, context, results, true);

        // Assert
        ClassicAssert.IsFalse(isValid);
        ClassicAssert.AreEqual(1, results.Count);
        ClassicAssert.AreEqual("Failed to parse InvalidEducation as Education",
            results[0].ErrorMessage);
    }

    [Test]
    public void Profile_ShouldFailValidation_WhenInvalidReligionIsProvided()
    {
        // Arrange
        var profile = new MatrimonyApiService.Profile.Profile
        {
            DateOfBirth = new DateTime(1990, 1, 1),
            Education = "NoEducation",
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "InvalidReligion",
            Ethnicity = "Asian",
            Gender = Gender.Male.ToString(),
            ManagedByRelation = Relation.Self.ToString(),
            MembershipId = 1,
            AnnualIncome = 100,
            Habit = "None",
            UserId = 1,
            PreferenceId = 1,
            ManagedById = 1,
            Height = 1,
            Weight = 2
        };

        var context = new ValidationContext(profile, null, null);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(profile, context, results, true);

        // Assert
        ClassicAssert.IsFalse(isValid);
        ClassicAssert.AreEqual(1, results.Count);
        ClassicAssert.AreEqual("Failed to parse InvalidReligion as Religion", results[0].ErrorMessage);
    }

    [Test]
    public void Profile_ShouldFailValidation_WhenInvalidGenderIsProvided()
    {
        // Arrange
        var profile = new MatrimonyApiService.Profile.Profile
        {
            DateOfBirth = new DateTime(1990, 1, 1),
            Education = Education.NoEducation.ToString(),
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Asian",
            Gender = "InvalidGender",
            ManagedByRelation = Relation.Self.ToString(),
            MembershipId = 1,
            AnnualIncome = 100,
            Habit = "None",
            UserId = 1,
            PreferenceId = 1,
            ManagedById = 1,
            Height = 1,
            Weight = 2
        };

        var context = new ValidationContext(profile, null, null);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(profile, context, results, true);

        // Assert
        ClassicAssert.IsFalse(isValid);
        ClassicAssert.AreEqual(1, results.Count);
        ClassicAssert.AreEqual("Failed to parse InvalidGender as Gender", results[0].ErrorMessage);
    }

    [Test]
    public void Profile_ShouldFailValidation_WhenInvalidMaritalStatusIsProvided()
    {
        // Arrange
        var profile = new MatrimonyApiService.Profile.Profile
        {
            DateOfBirth = new DateTime(1990, 1, 1),
            Education = Education.NoEducation.ToString(),
            Occupation = "Engineer",
            MaritalStatus = "InvalidStatus",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Asian",
            Gender = "Male",
            ManagedByRelation = Relation.Self.ToString(),
            MembershipId = 1,
            AnnualIncome = 100,
            Habit = "None",
            UserId = 1,
            PreferenceId = 1,
            ManagedById = 1,
            Height = 1,
            Weight = 2
        };

        var context = new ValidationContext(profile, null, null);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(profile, context, results, true);

        // Assert
        ClassicAssert.IsFalse(isValid);
        ClassicAssert.AreEqual(1, results.Count);
        ClassicAssert.AreEqual("Failed to parse InvalidStatus as MaritalStatus", results[0].ErrorMessage);
    }
}