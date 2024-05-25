using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrimonyTest.Membership
{
    using Microsoft.EntityFrameworkCore;
    using MatrimonyApiService.Entities;
    using MatrimonyApiService.Membership;
    using NUnit.Framework;
    using MatrimonyApiService.Commons;
    using NUnit.Framework.Legacy;
    using MatrimonyApiService.Enums;

    namespace MatrimonyTest.Membership
    {
        [TestFixture]
        public class MembershipRepoTests
        {
            private DbContextOptions<MatrimonyContext> _dbContextOptions;
            private MatrimonyContext _context;
            private MembershipRepo _membershipRepo;

            [SetUp]
            public void Setup()
            {
                _dbContextOptions = new DbContextOptionsBuilder<MatrimonyContext>()
                    .UseInMemoryDatabase(databaseName: "MatrimonyTestDb")
                    .Options;

                _context = new MatrimonyContext(_dbContextOptions);
                _membershipRepo = new MembershipRepo(_context);
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
                var membership = new MatrimonyApiService.Membership.Membership { Type = MemberShip.PremiumUser.ToString(), TypeEnum = MemberShip.PremiumUser, ProfileId = 1, Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false };
                await _context.Memberships.AddAsync(membership);
                await _context.SaveChangesAsync();

                // Act
                var result = await _membershipRepo.GetById(membership.Id);

                // Assert
                ClassicAssert.NotNull(result);
                ClassicAssert.AreEqual("PremiumUser", result.Type);
                ClassicAssert.AreEqual(MemberShip.PremiumUser, result.TypeEnum);
            }

            [Test]
            public void GetById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
            {
                // Act & Assert
                var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _membershipRepo.GetById(99));
                ClassicAssert.AreEqual("Membership with key 99 not found!!!", ex.Message);
            }

            [Test]
            public async Task GetAll_ShouldReturnAllEntities()
            {
                // Arrange
                await _context.Memberships.AddRangeAsync(
                    new MatrimonyApiService.Membership.Membership { Type = MemberShip.PremiumUser.ToString(), ProfileId = 1, Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false },
                    new MatrimonyApiService.Membership.Membership { Type = MemberShip.BasicUser.ToString(), ProfileId = 2, Description = "Basic membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = true }
                );
                await _context.SaveChangesAsync();

                // Act
                var result = await _membershipRepo.GetAll();

                // Assert
                ClassicAssert.AreEqual(2, result.Count);
            }

            [Test]
            public async Task Add_ShouldAddEntity()
            {
                // Arrange
                var membership = new MatrimonyApiService.Membership.Membership { Type = "Premium", ProfileId = 1, Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false };

                // Act
                var result = await _membershipRepo.Add(membership);

                // Assert
                ClassicAssert.IsNotNull(result);
                ClassicAssert.AreEqual("Premium", result.Type);
                ClassicAssert.AreEqual(1, await _context.Memberships.CountAsync());
            }

            [Test]
            public void Add_ShouldThrowArgumentNullException_WhenEntityIsNull()
            {
                // Act & Assert
                var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _membershipRepo.Add(null));
                ClassicAssert.AreEqual("Membership cannot be null. (Parameter 'entity')", ex.Message);
            }

            [Test]
            public async Task Update_ShouldUpdateEntity()
            {
                // Arrange
                var membership = new MatrimonyApiService.Membership.Membership { Type = "Premium", ProfileId = 1, Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false };
                await _context.Memberships.AddAsync(membership);
                await _context.SaveChangesAsync();

                membership.Description = "Updated Premium membership";

                // Act
                var result = await _membershipRepo.Update(membership);

                // Assert
                ClassicAssert.IsNotNull(result);
                ClassicAssert.AreEqual("Updated Premium membership", result.Description);
            }

            [Test]
            public async Task Update_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
            {
                // Arrange
                var updateMembership = new MatrimonyApiService.Membership.Membership { Id = 99, Type = "Basic", ProfileId = 1, Description = "Non-existent membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = true };

                // Act & Assert
                var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _membershipRepo.Update(updateMembership));
                ClassicAssert.AreEqual("Membership with key 99 not found!!!", ex.Message);
            }

            [Test]
            public async Task DeleteById_ShouldDeleteEntity()
            {
                // Arrange
                var membership = new MatrimonyApiService.Membership.Membership { Type = "Premium", ProfileId = 1, Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false };
                await _context.Memberships.AddAsync(membership);
                await _context.SaveChangesAsync();

                // Act
                await _membershipRepo.DeleteById(membership.Id);

                // Assert
                ClassicAssert.AreEqual(0, await _context.Memberships.CountAsync());
            }

            [Test]
            public void DeleteById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
            {
                // Act & Assert
                var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _membershipRepo.DeleteById(99));
                ClassicAssert.AreEqual("Membership with key 99 not found!!!", ex.Message);
            }
        }
    }
}
