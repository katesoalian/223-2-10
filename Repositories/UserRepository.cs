using AutoMapper;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories;

public class UserRepository : IUserRepository
{
    private readonly DepartmentContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DepartmentContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.Select(x => _mapper.Map<User>(x)).ToListAsync();
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return (await _context.Users.Select(x => _mapper.Map<User>(x)).ToListAsync()).FirstOrDefault(x => x.Id == id);
    }

    public async Task<string> SaveAsync(User model)
    {
        var entity = _mapper.Map<UserEntity>(model);
        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
        return model.Id;
    }

    public async Task UpdateAsync(User model)
    {
        var entity = _mapper.Map<UserEntity>(model);
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _context.Users.FindAsync(id);
        if (entity != null)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}