using ProductHub.Application.Abstractions.Services;
using ProductHub.Application.Common;
using ProductHub.Application.DTOs.Users;
using ProductHub.Application.Mappings;
using ProductHub.Application.Security;
using ProductHub.Domain.Entity.Exceptions;
using ProductHub.Domain.Entity.User;
using ProductHub.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<int>> CreateAsync(CreateUserRequest request)
    {
        var exists = await _userRepository.GetByUsernameAsync(request.Username);
        if (exists != null)
            return Result<int>.Failure("Username already exists.");

        var hash = _passwordHasher.Hash(request.Password);

        var user = new User(request.Username, hash);

        return Result<int>.Success(await _userRepository.InsertAsync(user));
    }

    public async Task<Result<IEnumerable<UserDto>>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        var dtos = users.Select(u => u.ToDto());

        return Result<IEnumerable<UserDto>>.Success(dtos);
    }

    public async Task<Result<UserDto>> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return Result<UserDto>.Failure("User not found.");

        return Result<UserDto>.Success(user.ToDto());
    }

    public async Task<Result> ChangePasswordAsync(int id, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return Result.Failure("User not found.");

        var hash = _passwordHasher.Hash(newPassword);

        user.ChangePassword(hash);

        await _userRepository.UpdateAsync(user);

        return Result.Success();
    }

    public async Task<Result> DeactivateAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return Result.Failure("User not found.");

        user.DeActivate();

        await _userRepository.UpdateAsync(user);

        return Result.Success();
    }
}
