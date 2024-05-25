using MatrimonyApiService.Commons;
using MatrimonyApiService.Enums;
using MatrimonyApiService.Preference;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;
using System.ComponentModel.DataAnnotations;

namespace MatrimonyTest.Preference;

    [TestFixture]
    public class PreferenceRepoTests
    {
        private DbContextOptions<MatrimonyContext> _dbContextOptions;
        private MatrimonyContext _context;
        private PreferenceRepo _preferenceRepo;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<MatrimonyContext>()
                .UseInMemoryDatabase(databaseName: "MatrimonyTestDb")
                .Options;

            _context = new MatrimonyContext(_dbContextOptions);
            _preferenceRepo = new PreferenceRepo(_context);
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
            await _context.Preferences.AddAsync(preference);
            await _context.SaveChangesAsync();

            // Act
            var result = await _preferenceRepo.GetById(preference.Id);

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(MotherTongue.English, result.MotherTongueEnum);
            ClassicAssert.AreEqual(MotherTongue.English.ToString(), result.MotherTongue);
            ClassicAssert.AreEqual(Education.NoEducation, result.EducationEnum);
            ClassicAssert.AreEqual(Education.NoEducation.ToString(), result.Education);
            ClassicAssert.AreEqual(Occupation.Engineer, result.OccupationEnum);
            ClassicAssert.AreEqual(Occupation.Engineer.ToString(), result.Occupation);
            ClassicAssert.AreEqual(Religion.Christian.ToString(), result.Religion);
            ClassicAssert.AreEqual(Religion.Christian, result.ReligionEnum);
        }

        [Test]
        public void GetById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _preferenceRepo.GetById(99));
            ClassicAssert.AreEqual("Preference with key 99 not found!!!", ex.Message);
        }

        [Test]
        public async Task GetAll_ShouldReturnAllEntities()
        {
            // Arrange
            await _context.Preferences.AddRangeAsync(
                new MatrimonyApiService.Preference.Preference
                {
                    MotherTongue = "English",
                    Religion = "Christian",
                    Education = "Graduate",
                    Occupation = "Engineer",
                    MinHeight = 160,
                    MaxHeight = 180,
                    MinAge = 25,
                    MaxAge = 35
                },
                new MatrimonyApiService.Preference.Preference
                {
                    MotherTongue = "Hindi",
                    Religion = "Hindu",
                    Education = "PostGraduate",
                    Occupation = "Doctor",
                    MinHeight = 150,
                    MaxHeight = 170,
                    MinAge = 28,
                    MaxAge = 38
                }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _preferenceRepo.GetAll();

            // Assert
            ClassicAssert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task Add_ShouldAddEntity()
        {
            // Arrange
            var preference = new MatrimonyApiService.Preference.Preference
            {
                MotherTongue = "English",
                Religion = "Christian",
                Education = "Graduate",
                Occupation = "Engineer",
                MinHeight = 160,
                MaxHeight = 180,
                MinAge = 25,
                MaxAge = 35
            };

            // Act
            var result = await _preferenceRepo.Add(preference);

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("English", result.MotherTongue);
            ClassicAssert.AreEqual(1, await _context.Preferences.CountAsync());
        }

        [Test]
        public void Add_ShouldThrowArgumentNullException_WhenEntityIsNull()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _preferenceRepo.Add(null));
            ClassicAssert.AreEqual("Preference cannot be null. (Parameter 'entity')", ex.Message);
        }

        [Test]
        public async Task Update_ShouldUpdateEntity()
        {
            // Arrange
            var preference = new MatrimonyApiService.Preference.Preference
            {
                MotherTongue = "English",
                Religion = "Christian",
                Education = "Graduate",
                Occupation = "Engineer",
                MinHeight = 160,
                MaxHeight = 180,
                MinAge = 25,
                MaxAge = 35
            };
            await _context.Preferences.AddAsync(preference);
            await _context.SaveChangesAsync();

            preference.Education = "PostGraduate";

            // Act
            var result = await _preferenceRepo.Update(preference);

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("PostGraduate", result.Education);
        }

        [Test]
        public async Task Update_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
        {
            // Arrange
            var updatePreference = new MatrimonyApiService.Preference.Preference
            {
                Id = 99,
                MotherTongue = "English",
                Religion = "Christian",
                Education = "Graduate",
                Occupation = "Engineer",
                MinHeight = 160,
                MaxHeight = 180,
                MinAge = 25,
                MaxAge = 35
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _preferenceRepo.Update(updatePreference));
            ClassicAssert.AreEqual("Preference with key 99 not found!!!", ex.Message);
        }

        [Test]
        public async Task DeleteById_ShouldDeleteEntity()
        {
            // Arrange
            var preference = new MatrimonyApiService.Preference.Preference
            {
                MotherTongue = "English",
                Religion = "Christian",
                Education = "Graduate",
                Occupation = "Engineer",
                MinHeight = 160,
                MaxHeight = 180,
                MinAge = 25,
                MaxAge = 35
            };
            await _context.Preferences.AddAsync(preference);
            await _context.SaveChangesAsync();

            // Act
            await _preferenceRepo.DeleteById(preference.Id);

            // Assert
            ClassicAssert.AreEqual(0, await _context.Preferences.CountAsync());
        }

        [Test]
        public void DeleteById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _preferenceRepo.DeleteById(99));
            ClassicAssert.AreEqual("Preference with key 99 not found!!!", ex.Message);
        }


    [Test]
    public void Preference_ShouldFailValidation_WhenInvalidReligionIsProvided()
    {
        // Arrange
        var preference = new MatrimonyApiService.Preference.Preference
        {
            MotherTongue = MotherTongue.English.ToString(),
            Religion = "InvalidReligion",
            Education = "InvalidEducation",
            Occupation = Occupation.Engineer.ToString(),
            MinHeight = 160,
            MaxHeight = 180,
            MinAge = 25,
            MaxAge = 35
        };

        var context = new ValidationContext(preference, null, null);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(preference, context, results, true);

        // Assert
        ClassicAssert.IsFalse(isValid);
        ClassicAssert.AreEqual(2, results.Count);
        ClassicAssert.AreEqual("Failed to parse InvalidReligion as Religion", results[0].ErrorMessage);
        ClassicAssert.AreEqual("Failed to parse InvalidEducation as Education", results[1].ErrorMessage);
    }

    [Test]
    public void Preference_ShouldPassValidation_WhenValidReligionIsProvided()
    {
        // Arrange
        var preference = new MatrimonyApiService.Preference.Preference
        {
            MotherTongue = MotherTongue.English.ToString(),
            Religion = Religion.Christian.ToString(),
            Education = Education.UG.ToString(),
            Occupation = Occupation.Doctor.ToString(),
            MinHeight = 160,
            MaxHeight = 180,
            MinAge = 25,
            MaxAge = 35
        };

        var context = new ValidationContext(preference, null, null);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(preference, context, results, true);

        // Assert
        ClassicAssert.IsTrue(isValid);
        ClassicAssert.IsEmpty(results);
    }
}