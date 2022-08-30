using AutoMapper;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly DepartmentContext _context;
    private readonly IMapper _mapper;

    public GroupRepository(DepartmentContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Group>> GetAllAsync()
    {
        return await _context.Groups.Select(x => _mapper.Map<Group>(x)).ToListAsync();
    }

    public async Task<Group?> GetByIdAsync(string id)
    {
        return (await _context.Groups.Select(x => _mapper.Map<Group>(x)).ToListAsync()).FirstOrDefault(x => x.Id == id);
    }

    public async Task<string> SaveAsync(Group model)
    {
        var entity = _mapper.Map<GroupEntity>(model);
        _context.Groups.Add(entity);
        await _context.SaveChangesAsync();
        return model.Id;
    }

    public async Task UpdateAsync(Group model)
    {
        var entity = _mapper.Map<GroupEntity>(model);
        _context.Groups.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _context.Groups.FindAsync(id);
        if (entity != null)
        {
            _context.Groups.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}