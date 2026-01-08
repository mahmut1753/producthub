using Microsoft.AspNetCore.Mvc;
using ProductHub.API.Common;
using ProductHub.Application.Abstractions.Services;
using ProductHub.Application.DTOs.Users;
using ProductHub.Application.Services;

namespace ProductHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IAuthService _authService;

    public LoginController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<BaseResponse<string>>> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (!result.IsSuccess)
            return BadRequest(BaseResponse<string>.Fail(result.Error));

        return Ok(BaseResponse<string>.Ok(result.Value!));
    }
}
