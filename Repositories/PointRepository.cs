using AutoMapper;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories;

public class PointRepository : IPointRepository
{
    private readonly DepartmentContext _context;
    private readonly IMapper _mapper;

    public PointRepository(DepartmentContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Points>> GetAllAsync()
    {
        return await _context.Points.Select(x => _mapper.Map<Points>(x)).ToListAsync();
    }

    public async Task<Points?> GetByIdAsync(string id)
    {
        return (await _context.Points.Select(x => _mapper.Map<Points>(x)).ToListAsync())
            .FirstOrDefault(x => x.Id == id);
    }

    public async Task<string> SaveAsync(Points model)
    {
        var entity = _mapper.Map<PointEntity>(model);
        _context.Points.Add(entity);
        await _context.SaveChangesAsync();
        return model.Id;
    }

    public async Task UpdateAsync(Points model)
    {
        var entity = _mapper.Map<PointEntity>(model);
        _context.Points.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _context.Points.FindAsync(id);
        if (entity != null)
        {
            _context.Points.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}