using ProductHub.Domain.Entity.Common;
using ProductHub.Domain.Entity.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Domain.Entity.User;

public class User : ActivatableEntity
{
    protected User() { }

    public User(string username, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username cannot be empty.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash cannot be empty.");

        Username = username;
        PasswordHash = passwordHash;
        IsActive = true;
        Role = (int)UserRole.User;//admin ilk kullanici
        CreatedAt = DateTime.UtcNow;
    }

    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public int Role { get; private set; }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("Password hash cannot be empty.");

        PasswordHash = newPasswordHash;
    }
}
