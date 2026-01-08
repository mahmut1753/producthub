using Dapper;
using ProductHub.Domain.Repositories;
using ProductHub.Domain.Entity.User;
using ProductHub.Infrastructure.Persistence;
using System.Data;

namespace ProductHub.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly IDapperContext _context;

    public UserRepository(IDapperContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string userName)
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            "AUTH.sp_User_GetByUsername",
            new { Username = userName },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            "AUTH.sp_User_GetById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<List<User>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();

        var users = await connection.QueryAsync<User>(
            "AUTH.sp_User_GetAll",
            commandType: CommandType.StoredProcedure);

        return users.ToList();
    }

    public async Task<int> InsertAsync(User user)
    {
        using var connection = _context.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            "AUTH.sp_User_Insert",
            new
            {
                user.Username,
                user.PasswordHash,
                user.Role,
                user.IsActive
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateAsync(User user)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            "AUTH.sp_User_Update",
            new
            {
                user.Id,
                user.PasswordHash,
                user.IsActive
            },
            commandType: CommandType.StoredProcedure);
    }
}