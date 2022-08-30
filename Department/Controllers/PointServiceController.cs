using AutoMapper;
using Core.Models;
using Core.Views;
using Logic;
using Microsoft.AspNetCore.Mvc;

namespace Department.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PointServiceController : ControllerBase
{
    private readonly IPointsService _service;
    
    public PointServiceController(IPointsService service)
    {
        _service = service;
    }

    [HttpGet("{studentId}/points")]
    public async Task<IEnumerable<Points?>> GetPoints(string studentId)
    {
        return await _service.GetPointsByStudentIdAsync(studentId);
    }

    [HttpPost]
    public async Task Post(string studentId, [FromBody] PointsView points)
    {
        await _service.AddPointToStudentAsync(studentId,
            new Points() { Id = Guid.NewGuid().ToString(), Subject = points.Subject, Point = points.Point });
    }

    [HttpDelete]
    public async Task Delete(string relationId)
    {
        await _service.DeletePointFromStudentAsync(relationId);
    }
}