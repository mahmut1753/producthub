using ProductHub.Application.Common;
using ProductHub.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.Abstractions.Services;

public interface IAuthService
{
    Task<Result<string>> LoginAsync(LoginRequest request);
}
