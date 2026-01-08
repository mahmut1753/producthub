using ProductHub.Application.Common;
using ProductHub.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.Abstractions.Services;

public interface IUserService
{
    Task<Result<int>> CreateAsync(CreateUserRequest request);
    Task<Result<IEnumerable<UserDto>>> GetAllAsync();
    Task<Result<UserDto>> GetByIdAsync(int id);
    Task<Result> ChangePasswordAsync(int id, string newPassword);
    Task<Result> DeactivateAsync(int id);
}
