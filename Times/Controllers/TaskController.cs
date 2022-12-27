using Microsoft.AspNetCore.Mvc;
using Times.Infrastructure.Entities;

namespace Times.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{

    public IEnumerable<TaskEntity> GetTasks()
    {
        return Enumerable.Range(1, 5).Select(i => new TaskEntity
        {
            Title = "Task " + i,
            Description = "Task Description " + i
        })
        .ToArray();
    }
}
