using ProductHub.Application.DTOs.Users;
using ProductHub.Domain.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.Mappings;

public static class UserMapping
{
    public static UserDto ToDto(this User user)
    {
        if (user is null)
            return null;

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}
