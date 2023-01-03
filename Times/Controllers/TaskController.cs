using Microsoft.AspNetCore.Mvc;
using Times.Infrastructure.Entities;

namespace Times.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TaskController : ControllerBase
{

    [HttpGet]
    public IEnumerable<TaskEntity> Tasks()
    {
        return Enumerable.Range(1, 5).Select(i => new TaskEntity
        {
            Id = i,
            Title = "Task " + i,
            Description = "Task Description " + i
        })
        .ToArray();
    }
}
