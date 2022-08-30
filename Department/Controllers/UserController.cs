using Core.Models;
using Core.Views;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace Department.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> Get()
    {
        return await _userRepository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<User?> Get(string id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task Post([FromBody] UserView user)
    {
        await _userRepository.SaveAsync(new User()
        {
            Id = Guid.NewGuid().ToString(),
            Login = user.Login,
            Password = user.Password
        });
    }

    [HttpDelete]
    public async Task Delete(string id)
    {
        await _userRepository.DeleteAsync(id);
    }
}