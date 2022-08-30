using AutoMapper;
using Core.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repositories.Entities;
using Repositories.Interfaces;
using Xunit;

namespace Repositories.Tests;

public class GroupRepositoryTests
{
    private readonly IGroupRepository _groupRepository;
    private readonly DepartmentContext _context;
    private readonly Mock<IMapper> _mapperMock;

    public GroupRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DepartmentContext>()
            .UseInMemoryDatabase(databaseName: "In-Memory Department").Options;

        _context = new DepartmentContext(options);

        _mapperMock = new Mock<IMapper>();
        _groupRepository = new GroupRepository(_context, _mapperMock.Object);
    }
    
    
    [Fact]
    public async Task GetAllAsync_ShouldContainExistingObjectInCollection()
    {
        // Arrange
        var groupId = Guid.NewGuid().ToString();
        var groupEntity = new GroupEntity() { Id = groupId, Course = 3, Name = "323", ProfessorId = "ProfessorId" };
        var group = new Group() { Id = groupId, Course = 3, Name = "323", ProfessorId = "ProfessorId" };

        _context.Groups.Add(groupEntity);
        await _context.SaveChangesAsync();
        
        _mapperMock.Setup(_ => _.Map<Group>(groupEntity)).Returns(group);

        // Act
        var result = await _groupRepository.GetAllAsync();

        // Assert
        result.Should().Contain(group);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnExistingObjectById()
    {
        // Arrange
        var groupId = Guid.NewGuid().ToString();
        var groupEntity = new GroupEntity() { Id = groupId, Course = 3, Name = "323", ProfessorId = "ProfessorId" };
        var group = new Group() { Id = groupId, Course = 3, Name = "323", ProfessorId = "ProfessorId" };

        _context.Groups.Add(groupEntity);
        await _context.SaveChangesAsync();

        _mapperMock.Setup(_ => _.Map<Group>(groupEntity)).Returns(group);
        
        // Act
        var result = await _groupRepository.GetByIdAsync(groupId);

        // Assert
        result.Should().BeEquivalentTo(group);
    }

    [Fact]
    public async Task SaveAsync_ReturnExistingObjectWithSameId()
    {
        // Arrange
        var groupId = Guid.NewGuid().ToString();
        var groupEntity = new GroupEntity() { Id = groupId, Course = 3, Name = "323", ProfessorId = "ProfessorId" };
        var group = new Group() { Id = groupId, Course = 3, Name = "323", ProfessorId = "ProfessorId" };
        
        _mapperMock.Setup(_ => _.Map<GroupEntity>(group)).Returns(groupEntity);
        
        // Act
        var resultId = await _groupRepository.SaveAsync(group);
        var result = await _context.Groups.FindAsync(groupId);

        // Assert
        resultId.Should().BeEquivalentTo(groupId);
        result.Should().BeEquivalentTo(groupEntity);
    }

    [Fact]
    public async Task DeleteAsync_ReturnNullAfterDeletingObject()
    {
        // Arrange
        var groupId = Guid.NewGuid().ToString();
        var groupEntity = new GroupEntity() { Id = groupId, Course = 3, Name = "323", ProfessorId = "ProfessorId" };
        var group = new Group() { Id = groupId, Course = 3, Name = "323", ProfessorId = "ProfessorId" };
        
        _context.Groups.Add(groupEntity);
        await _context.SaveChangesAsync();

        // Act
        await _groupRepository.DeleteAsync(groupId);
        var result = await _context.Users.FindAsync(groupId);

        // Assert
        result.Should().BeNull();
    }
}