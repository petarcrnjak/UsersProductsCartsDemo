using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Application.Auth;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken cancellation = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellation = default);
    Task<User> AddAsync(User user, CancellationToken cancellation = default);
    Task<bool> ExistsAsync(string email, string username, CancellationToken cancellation = default);
}
