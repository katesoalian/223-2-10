using AutoMapper;
using Core.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repositories.Entities;
using Repositories.Interfaces;
using Xunit;

namespace Repositories.Tests;

public class StudentRepositoryTests
{
    private readonly IStudentRepository _studentRepository;
    private readonly DepartmentContext _context;
    private readonly Mock<IMapper> _mapperMock;

    public StudentRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DepartmentContext>()
            .UseInMemoryDatabase(databaseName: "In-Memory Department").Options;

        _context = new DepartmentContext(options);

        _mapperMock = new Mock<IMapper>();
        _studentRepository = new StudentRepository(_context, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldContainExistingObjectInCollection()
    {
        // Arrange
        var studentId = Guid.NewGuid().ToString();
        var studentEntity = new StudentEntity()
        {
            Id = studentId, FirstName = "Alex", LastName = "Alexeev", GroupId = "GroupId"
        };
        var student = new Student() { Id = studentId, FirstName = "Alex", LastName = "Alexeev", GroupId = "GroupId" };

        _context.Students.Add(studentEntity);
        await _context.SaveChangesAsync();
        
        _mapperMock.Setup(_ => _.Map<Student>(studentEntity)).Returns(student);

        // Act
        var result = await _studentRepository.GetAllAsync();

        // Assert
        result.Should().Contain(student);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnExistingObjectById()
    {
        // Arrange
        var studentId = Guid.NewGuid().ToString();
        var studentEntity = new StudentEntity()
        {
            Id = studentId, FirstName = "Alex", LastName = "Alexeev", GroupId = "GroupId"
        };
        var student = new Student() { Id = studentId, FirstName = "Alex", LastName = "Alexeev", GroupId = "GroupId" };

        _context.Students.Add(studentEntity);
        await _context.SaveChangesAsync();

        _mapperMock.Setup(_ => _.Map<Student>(studentEntity)).Returns(student);
        
        // Act
        var result = await _studentRepository.GetByIdAsync(studentId);

        // Assert
        result.Should().BeEquivalentTo(student);
    }

    [Fact]
    public async Task SaveAsync_ReturnExistingObjectWithSameId()
    {
        // Arrange
        var studentId = Guid.NewGuid().ToString();
        var studentEntity = new StudentEntity()
        {
            Id = studentId, FirstName = "Alex", LastName = "Alexeev", GroupId = "GroupId"
        };

        var student = new Student() { Id = studentId, FirstName = "Alex", LastName = "Alexeev", GroupId = "GroupId" };
        
        _mapperMock.Setup(_ => _.Map<StudentEntity>(student)).Returns(studentEntity);
        
        // Act
        var resultId = await _studentRepository.SaveAsync(student);
        var result = await _context.Students.FindAsync(studentId);

        // Assert
        resultId.Should().BeEquivalentTo(studentId);
        result.Should().BeEquivalentTo(studentEntity);
    }

    [Fact]
    public async Task DeleteAsync_ReturnNullAfterDeletingObject()
    {
        // Arrange
        var studentId = Guid.NewGuid().ToString();
        var studentEntity = new StudentEntity()
        {
            Id = studentId, FirstName = "Alex", LastName = "Alexeev", GroupId = "GroupId"
        };
        _context.Students.Add(studentEntity);
        await _context.SaveChangesAsync();

        // Act
        await _studentRepository.DeleteAsync(studentId);
        var result = await _context.Students.FindAsync(studentId);

        // Assert
        result.Should().BeNull();
    }
}