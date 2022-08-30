using Core.Models;
using Core.Views;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace Department.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IStudentRepository _studentRepository;

    public StudentController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<Student>> Get()
    {
        return await _studentRepository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<Student?> GetById(string id)
    {
        return await _studentRepository.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task Post([FromBody] StudentView student)
    {
        await _studentRepository.SaveAsync(new Student()
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = student.FirstName,
            LastName = student.LastName,
            GroupId = student.GroupId
        });
    }

    [HttpDelete]
    public async Task Delete(string id)
    {
        await _studentRepository.DeleteAsync(id);
    }
}