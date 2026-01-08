using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductHub.API.Common;
using ProductHub.Application.Abstractions.Services;
using ProductHub.Application.DTOs.Users;
using ProductHub.Application.Services;

namespace ProductHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<BaseResponse<int>> Create(CreateUserRequest request)
    {
        var result = await _userService.CreateAsync(request);

        if (!result.IsSuccess)
            return BaseResponse<int>.Fail(result.Error!);

        return BaseResponse<int>.Ok(result.Value);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<BaseResponse<IEnumerable<UserDto>>> GetAll()
    {
        var result = await _userService.GetAllAsync();

        return result.IsSuccess
            ? BaseResponse<IEnumerable<UserDto>>.Ok(result.Value)
            : BaseResponse<IEnumerable<UserDto>>.Fail(result.Error!);
    }

    [HttpGet("{id:int}")]
    public async Task<BaseResponse<UserDto>> GetById(int id)
    {
        var result = await _userService.GetByIdAsync(id);

        return result.IsSuccess
            ? BaseResponse<UserDto>.Ok(result.Value)
            : BaseResponse<UserDto>.Fail(result.Error!);
    }

    [HttpPut("{id:int}/password")]
    public async Task<BaseResponse> ChangePassword(int id, [FromBody] ChangePasswordRequest request)
    {
        var result = await _userService.ChangePasswordAsync(id, request.NewPassword);

        return result.IsSuccess
            ? BaseResponse.Ok()
            : BaseResponse.Fail(result.Error!);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<BaseResponse> Deactivate(int id)
    {
        var result = await _userService.DeactivateAsync(id);

        return result.IsSuccess
            ? BaseResponse.Ok()
            : BaseResponse.Fail(result.Error!);
    }
}
