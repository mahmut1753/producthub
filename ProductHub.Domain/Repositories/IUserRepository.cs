using ProductHub.Domain.Entity.User;

namespace ProductHub.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> GetAllAsync();
    Task<int> InsertAsync(User user);
    Task UpdateAsync(User user);
}