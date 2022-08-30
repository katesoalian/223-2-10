using AutoMapper;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories;

public class RelationRepository : IRelationRepository
{
    private readonly DepartmentContext _context;
    private readonly IMapper _mapper;

    public RelationRepository(DepartmentContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Student_Point>> GetAllAsync()
    {
        return await _context.StudentPoints.Select(x => _mapper.Map<Student_Point>(x)).ToListAsync();
    }

    public async Task<Student_Point?> GetByIdAsync(string id)
    {
        return (await _context.StudentPoints.Select(x => _mapper.Map<Student_Point>(x)).ToListAsync()).FirstOrDefault(
            x => x.Id == id);
    }

    public async Task<string> SaveAsync(Student_Point model)
    {
        var entity = _mapper.Map<StudentEntity_PointEntity>(model);
        _context.StudentPoints.Add(entity);
        await _context.SaveChangesAsync();
        return model.Id;
    }

    public async Task UpdateAsync(Student_Point model)
    {
        var entity = _mapper.Map<StudentEntity_PointEntity>(model);
        _context.StudentPoints.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _context.StudentPoints.FindAsync(id);
        if (entity != null)
        {
            _context.StudentPoints.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Student_Point>> GetByStudentIdAsync(string id)
    {
        return (await _context.StudentPoints.Select(x => _mapper.Map<Student_Point>(x)).ToListAsync())
            .Where(x => x.StudentId == id).ToList();
    }

    public async Task<List<Student_Point>> GetByPointIdAsync(string id)
    {
        return (await _context.StudentPoints.Select(x => _mapper.Map<Student_Point>(x)).ToListAsync())
            .Where(x => x.PointId == id).ToList();
    }
}