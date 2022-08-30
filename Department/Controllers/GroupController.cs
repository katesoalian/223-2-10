using Core.Models;
using Core.Views;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace Department.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupController : ControllerBase
{
    private readonly IGroupRepository _groupRepository;

    public GroupController(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<Group>> Get()
    {
        return await _groupRepository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<Group?> Get(string id)
    {
        return await _groupRepository.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task Post([FromBody] GroupView group)
    {
        await _groupRepository.SaveAsync(new Group()
            {
                Id = Guid.NewGuid().ToString(),
                Name = group.Name,
                Course = group.Course,
                ProfessorId = group.ProfessorId
            });
    }

    [HttpDelete]
    public async Task Delete(string id)
    {
        await _groupRepository.DeleteAsync(id);
    }
}