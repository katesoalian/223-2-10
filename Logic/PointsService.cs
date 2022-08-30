using System.Drawing;
using Core.Models;
using Repositories.Interfaces;

namespace Logic;

public class PointsService : IPointsService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IPointRepository _pointRepository;
    private readonly IRelationRepository _relationRepository;


    public PointsService(IStudentRepository studentRepository, IPointRepository pointRepository, IRelationRepository relationRepository)
    {
        _studentRepository = studentRepository;
        _pointRepository = pointRepository;
        _relationRepository = relationRepository;
    }

    public async Task<List<Points?>> GetPointsByStudentIdAsync(string studentId)
    {
        var relations = await _relationRepository.GetByStudentIdAsync(studentId);
        var points = new List<Points?>();
        foreach (var relation in relations)
        {
            var point = await _pointRepository.GetByIdAsync(relation.PointId);
            points.Add(point);
        }
        return points;
    }

    public async Task AddPointToStudentAsync(string studentId, Points point)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student != null)
        {
            var relation = new Student_Point()
            {
                Id = Guid.NewGuid().ToString(),
                StudentId = student.Id,
                PointId = await _pointRepository.SaveAsync(point)
            };

            await _relationRepository.SaveAsync(relation);
        }
    }

    public async Task DeletePointFromStudentAsync(string relationId)
    {
        var relation = await _relationRepository.GetByIdAsync(relationId);
        if (relation != null)
        {
            var pointId = relation.PointId;

            await _relationRepository.DeleteAsync(relationId);
            await _pointRepository.DeleteAsync(pointId);
        }
    }
}