using ProductHub.Application.Abstractions.Services;
using ProductHub.Application.Common;
using ProductHub.Application.DTOs.Users;
using ProductHub.Application.Security;
using ProductHub.Domain.Entity.Exceptions;
using ProductHub.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<Result<string>> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.UserName);

        if (user is null)
            return Result<string>.Failure("Invalid username or password.");
        
        if (!user.IsActive)
            return Result<string>.Failure("User is inactive.");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result<string>.Failure("Invalid username or password.");

        return Result<string>.Success(_tokenService.GenerateToken(user));
    }
}