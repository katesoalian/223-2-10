using AutoMapper;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly DepartmentContext _context;
    private readonly IMapper _mapper;

    public StudentRepository(DepartmentContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Students.Select(x => _mapper.Map<Student>(x)).ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(string id)
    {
        return (await _context.Students.Select(x => _mapper.Map<Student>(x)).ToListAsync()).FirstOrDefault(x =>
            x.Id == id);
    }

    public async Task<string> SaveAsync(Student model)
    {
        var entity = _mapper.Map<StudentEntity>(model);
        _context.Students.Add(entity);
        await _context.SaveChangesAsync();
        return model.Id;
    }

    public async Task UpdateAsync(Student model)
    {
        var entity = _mapper.Map<StudentEntity>(model);
        _context.Students.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _context.Students.FindAsync(id);
        if (entity != null)
        {
            _context.Students.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}