using Core.Models;
using Core.Views;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace Department.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfessorController : ControllerBase
{
    private readonly IProfessorRepository _professorRepository;

    public ProfessorController(IProfessorRepository professorRepository)
    {
        _professorRepository = professorRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<Professor>> Get()
    {
        return await _professorRepository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<Professor?> Get(string id)
    {
        return await _professorRepository.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task Post([FromBody] ProfessorView professor)
    {
        await _professorRepository.SaveAsync(new Professor()
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = professor.FirstName,
            LastName = professor.LastName,
            Subject = professor.Subject
        });
    }

    [HttpDelete]
    public async Task Delete(string id)
    {
        await _professorRepository.DeleteAsync(id);
    }
}