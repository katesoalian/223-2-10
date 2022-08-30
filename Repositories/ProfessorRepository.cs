using AutoMapper;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories;

public class ProfessorRepository : IProfessorRepository
{
    private readonly DepartmentContext _context;
    private readonly IMapper _mapper;

    public ProfessorRepository(DepartmentContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Professor>> GetAllAsync()
    {
        return await _context.Professors.Select(x => _mapper.Map<Professor>(x)).ToListAsync();
    }

    public async Task<Professor?> GetByIdAsync(string id)
    {
        return (await _context.Professors.Select(x => _mapper.Map<Professor>(x)).ToListAsync())
            .FirstOrDefault(x => x.Id == id);
    }

    public async Task<string> SaveAsync(Professor model)
    {
        var entity = _mapper.Map<ProfessorEntity>(model);
        _context.Professors.Add(entity);
        await _context.SaveChangesAsync();
        return model.Id;
    }

    public async Task UpdateAsync(Professor model)
    {
        var entity = _mapper.Map<ProfessorEntity>(model);
        _context.Professors.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _context.Professors.FindAsync(id);
        if (entity != null)
        {
            _context.Professors.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}