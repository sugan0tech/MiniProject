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
            .UseInMemoryDatabase(databaseName: "MatrimonyTestDb")
            .Options;

        _context = new MatrimonyContext(_dbContextOptions);
        _profileRepo = new ProfileRepo(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    // [Test]
    // public async Task GetById_ShouldReturnEntity_WhenEntityExists()
    // {
    //     // Arrange
    //     var profile = new MatrimonyApiService.Profile.Profile
    //     {
    //         DateOfBirth = new DateTime(1990, 1, 1),
    //         Education = Education.NoEducation.ToString(),
    //         Occupation = Occupation.Engineer.ToString(),
    //         MaritalStatus = MaritalStatus.Single.ToString(),
    //         MotherTongue = MotherTongue.English.ToString(),
    //         Religion = Religion.Christian.ToString(),
    //         Ethnicity = Ethnicity.Indian.ToString(),
    //         Gender = Gender.Male.ToString(),
    //         ManagedByRelation = "Self",
    //         MembershipId = 1,
    //         AnnualIncome = 100,
    //         Habits = true,
    //         UserId = 1,
    //         PreferenceId = 2,
    //         UpdatedAt = DateTime.Now
    //     };
    //
    //     await _context.Profiles.AddAsync(profile);
    //     await _context.SaveChangesAsync();
    //
    //     // Act
    //     var result = await _profileRepo.GetById(profile.Id);
    //
    //     // Assert
    //     ClassicAssert.NotNull(result);
    //     ClassicAssert.AreEqual(Education.NoEducation, result.Education);
    //     ClassicAssert.AreEqual(Occupation.Engineer.ToString(), result.Occupation);
    // }

    [Test]
    public void GetById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _profileRepo.GetById(99));
        ClassicAssert.AreEqual("Profile with key 99 not found!!!", ex.Message);
    }

    // [Test]
    // public async Task GetAll_ShouldReturnAllEntities()
    // {
    //     // Arrange
    //     await _context.Profiles.AddRangeAsync(
    //         new MatrimonyApiService.Profile.Profile
    //         {
    //             DateOfBirth = new DateTime(1990, 1, 1),
    //             Education = "Graduate",
    //             Occupation = "Engineer",
    //             MaritalStatus = "Single",
    //             MotherTongue = "English",
    //             Religion = "Christian",
    //             Ethnicity = "Asian",
    //             Gender = "Male",
    //             ManagedByRelation = "Self",
    //             MembershipId = 1,
    //             UserId = 1
    //         },
    //         new MatrimonyApiService.Profile.Profile
    //         {
    //             DateOfBirth = new DateTime(1985, 5, 5),
    //             Education = "PostGraduate",
    //             Occupation = "Doctor",
    //             MaritalStatus = "Divorced",
    //             MotherTongue = "Hindi",
    //             Religion = "Hindu",
    //             Ethnicity = "Asian",
    //             Gender = "Female",
    //             ManagedByRelation = "Parent",
    //             MembershipId = 2,
    //             UserId = 2
    //         }
    //     );
    //     await _context.SaveChangesAsync();
    //
    //     // Act
    //     var result = await _profileRepo.GetAll();
    //
    //     // Assert
    //     ClassicAssert.AreEqual(2, result.Count);
    // }

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
            Gender = "Male",
            ManagedByRelation = "Self",
            MembershipId = 1,
            UserId = 1
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

    // [Test]
    // public async Task Update_ShouldUpdateEntity()
    // {
    //     // Arrange
    //     var profile = new MatrimonyApiService.Profile.Profile
    //     {
    //         DateOfBirth = new DateTime(1990, 1, 1),
    //         Education = "Graduate",
    //         Occupation = "Engineer",
    //         MaritalStatus = "Single",
    //         MotherTongue = "English",
    //         Religion = "Christian",
    //         Ethnicity = "Asian",
    //         Gender = "Male",
    //         ManagedByRelation = "Self",
    //         MembershipId = 1,
    //         UserId = 1
    //     };
    //     await _context.Profiles.AddAsync(profile);
    //     await _context.SaveChangesAsync();
    //
    //     profile.Education = "PostGraduate";
    //
    //     // Act
    //     var result = await _profileRepo.Update(profile);
    //
    //     // Assert
    //     ClassicAssert.IsNotNull(result);
    //     ClassicAssert.AreEqual("PostGraduate", result.Education);
    // }

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
            Gender = "Male",
            ManagedByRelation = "Self",
            MembershipId = 1,
            UserId = 1
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _profileRepo.Update(updateProfile));
        ClassicAssert.AreEqual("Profile with key 99 not found!!!", ex.Message);
    }

    // [Test]
    // public async Task DeleteById_ShouldDeleteEntity()
    // {
    //     // Arrange
    //     var profile = new MatrimonyApiService.Profile.Profile
    //     {
    //         DateOfBirth = new DateTime(1990, 1, 1),
    //         Education = "Graduate",
    //         Occupation = "Engineer",
    //         MaritalStatus = "Single",
    //         MotherTongue = "English",
    //         Religion = "Christian",
    //         Ethnicity = "Asian",
    //         Gender = "Male",
    //         ManagedByRelation = "Self",
    //         MembershipId = 1,
    //         UserId = 1
    //     };
    //     await _context.Profiles.AddAsync(profile);
    //     await _context.SaveChangesAsync();
    //
    //     // Act
    //     await _profileRepo.DeleteById(profile.Id);
    //
    //     // Assert
    //     ClassicAssert.AreEqual(0, await _context.Profiles.CountAsync());
    // }

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
            Gender = "Male",
            ManagedByRelation = "Self",
            MembershipId = 1,
            UserId = 1
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
            Gender = "Male",
            ManagedByRelation = "Self",
            MembershipId = 1,
            UserId = 1
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
            ManagedByRelation = "Self",
            MembershipId = 1,
            UserId = 1
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
            ManagedByRelation = "Self",
            MembershipId = 1,
            UserId = 1
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