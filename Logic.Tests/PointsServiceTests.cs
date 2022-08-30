using Core.Models;
using FluentAssertions;
using Moq;
using Repositories.Interfaces;
using Xunit;

namespace Logic.Tests;

public class PointsServiceTests
{
    private readonly IPointsService _service;
    private readonly Mock<IPointRepository> _pointRepositoryMock;
    private readonly Mock<IRelationRepository> _relationRepositoryMock;

    public PointsServiceTests()
    {
        _pointRepositoryMock = new Mock<IPointRepository>();
        _relationRepositoryMock = new Mock<IRelationRepository>();

        var studentRepositoryMock = new Mock<IStudentRepository>();
        
        _service = new PointsService(studentRepositoryMock.Object,
            _pointRepositoryMock.Object,
            _relationRepositoryMock.Object);
    }

    [Fact]
    public async Task GetPointsByStudentIdAsync_ShouldContainExistingObject()
    {
        // Arrange
        const string studentId = "StudentId";
        var point = new Points() { Id = "PointId", Point = 20, Subject = "Math" };
        var relation = new Student_Point() { Id = "RelationId", PointId = "PointId", StudentId = "StudentId" };
        
        _relationRepositoryMock.Setup(_ => _.GetByStudentIdAsync(studentId))
            .ReturnsAsync(new List<Student_Point>() { relation });
        _pointRepositoryMock.Setup(_ => _.GetByIdAsync(relation.PointId)).ReturnsAsync(point);
        
        // Act
        var result  = await _service.GetPointsByStudentIdAsync(studentId);
        
        // Assert
        result.Should().Contain(point);
    }
}