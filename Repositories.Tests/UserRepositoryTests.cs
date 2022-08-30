using AutoMapper;
using Core.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repositories.Entities;
using Repositories.Interfaces;
using Xunit;

namespace Repositories.Tests;

public class UserRepositoryTests
{
    private readonly IUserRepository _userRepository;
    private readonly DepartmentContext _context;
    private readonly Mock<IMapper> _mapperMock;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DepartmentContext>()
            .UseInMemoryDatabase(databaseName: "In-Memory Department").Options;

        _context = new DepartmentContext(options);

        _mapperMock = new Mock<IMapper>();
        _userRepository = new UserRepository(_context, _mapperMock.Object);
    }
    
    
    [Fact]
    public async Task GetAllAsync_ShouldContainExistingObjectInCollection()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var userEntity = new UserEntity() { Id = userId, Login = "admin", Password = "123" };
        var user = new User() { Id = userId, Login = "admin", Password = "123" };
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        _mapperMock.Setup(_ => _.Map<User>(userEntity)).Returns(user);
        
        // Act
        var result = await _userRepository.GetAllAsync();

        // Assert
        result.Should().Contain(user);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnExistingObjectById()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var userEntity = new UserEntity() { Id = userId, Login = "admin", Password = "123" };
        var user = new User() { Id = userId, Login = "admin", Password = "123" };

        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        _mapperMock.Setup(_ => _.Map<User>(userEntity)).Returns(user);
        
        // Act
        var result = await _userRepository.GetByIdAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task SaveAsync_ReturnExistingObjectWithSameId()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var userEntity = new UserEntity() { Id = userId, Login = "admin", Password = "123" };
        var user = new User() { Id = userId, Login = "admin", Password = "123" };
        
        _mapperMock.Setup(_ => _.Map<UserEntity>(user)).Returns(userEntity);
        
        // Act
        var resultId = await _userRepository.SaveAsync(user);
        var result = await _context.Users.FindAsync(userId);

        // Assert
        resultId.Should().BeEquivalentTo(userId);
        result.Should().BeEquivalentTo(userEntity);
    }

    [Fact]
    public async Task DeleteAsync_ReturnNullAfterDeletingObject()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var userEntity = new UserEntity() { Id = userId, Login = "admin", Password = "123" };
        var user = new User() { Id = userId, Login = "admin", Password = "123" };

        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        // Act
        await _userRepository.DeleteAsync(userId);
        var result = await _context.Users.FindAsync(userId);

        // Assert
        result.Should().BeNull();
    }
}