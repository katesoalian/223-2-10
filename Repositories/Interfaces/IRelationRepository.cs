using Core.Models;

namespace Repositories.Interfaces;

public interface IRelationRepository : IRepository<Student_Point>
{
    Task<List<Student_Point>> GetByStudentIdAsync(string id);
    Task<List<Student_Point>> GetByPointIdAsync(string id);
}