using Core.Models;

namespace Logic;

public interface IPointsService
{
    public Task<List<Points?>> GetPointsByStudentIdAsync(string studentId);
    public Task AddPointToStudentAsync(string studentId, Points point);
    public Task DeletePointFromStudentAsync(string relationId);
}