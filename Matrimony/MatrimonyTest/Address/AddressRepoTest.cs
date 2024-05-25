using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MatrimonyApiService.Address;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Repos;

namespace MatrimonyTest.Address
{
    [TestFixture]
    public class AddressRepoTests
    {
        private DbContextOptions<MatrimonyContext> _dbContextOptions;
        private MatrimonyContext _context;
        private AddressRepo _addressRepo;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<MatrimonyContext>()
                .UseInMemoryDatabase(databaseName: "MatrimonyTestDb")
                .Options;

            _context = new MatrimonyContext(_dbContextOptions);
            _addressRepo = new AddressRepo(_context);
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
            var address = new Address { Id = 1, City = "City1", State = "State1", Country = "Country1" };
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();

            // Act
            var result = await _addressRepo.GetById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("City1", result.City);
        }

        [Test]
        public void GetById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _addressRepo.GetById(99));
            Assert.AreEqual("Address with key 99 not found!!!", ex.Message);
        }

        [Test]
        public async Task GetAll_ShouldReturnAllEntities()
        {
            // Arrange
            await _context.Addresses.AddRangeAsync(
                new Address { Id = 1, City = "City1", State = "State1", Country = "Country1" },
                new Address { Id = 2, City = "City2", State = "State2", Country = "Country2" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _addressRepo.GetAll();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task Add_ShouldAddEntity()
        {
            // Arrange
            var address = new Address { Id = 1, City = "City1", State = "State1", Country = "Country1" };

            // Act
            var result = await _addressRepo.Add(address);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("City1", result.City);
            Assert.AreEqual(1, await _context.Addresses.CountAsync());
        }

        [Test]
        public void Add_ShouldThrowArgumentNullException_WhenEntityIsNull()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _addressRepo.Add(null));
            Assert.AreEqual("Address cannot be null. (Parameter 'entity')", ex.Message);
        }

        [Test]
        public async Task Update_ShouldUpdateEntity()
        {
            // Arrange
            var address = new Address { Id = 1, City = "City1", State = "State1", Country = "Country1" };
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();

            var updateAddress = new Address { Id = 1, City = "NewCity", State = "NewState", Country = "NewCountry" };

            // Act
            var result = await _addressRepo.Update(updateAddress);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("NewCity", result.City);
        }

        [Test]
        public async Task Update_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
        {
            // Arrange
            var updateAddress = new Address { Id = 99, City = "City99", State = "State99", Country = "Country99" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _addressRepo.Update(updateAddress));
            Assert.AreEqual("Address with key 99 not found!!!", ex.Message);
        }

        [Test]
        public async Task DeleteById_ShouldDeleteEntity()
        {
            // Arrange
            var address = new Address { Id = 1, City = "City1", State = "State1", Country = "Country1" };
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();

            // Act
            await _addressRepo.DeleteById(1);

            // Assert
            Assert.AreEqual(0, await _context.Addresses.CountAsync());
        }

        [Test]
        public void DeleteById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _addressRepo.DeleteById(99));
            Assert.AreEqual("Address with key 99 not found!!!", ex.Message);
        }
    }
}
