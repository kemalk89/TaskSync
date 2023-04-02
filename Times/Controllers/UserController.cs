using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Times.Controllers.Request;
using Times.Controllers.Response;
using Times.Domain.User;

namespace Times.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class UserController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IEnumerable<UserResponse>> SearchUsersAsync([FromBody] SearchUserRequest req)
    {
        var users = await _userService.SearchUserAsync(req.ToCommand());
        return users.Select(item => new UserResponse(item));
    }
}
