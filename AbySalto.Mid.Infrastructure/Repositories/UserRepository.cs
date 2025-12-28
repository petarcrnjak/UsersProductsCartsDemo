using AbySalto.Mid.Application.Auth;
using AbySalto.Mid.Domain.Entities;
using AbySalto.Mid.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.Mid.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellation = default)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellation);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellation = default)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellation);
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellation = default)
    {
        await _context.Users.AddAsync(user, cancellation);
        await _context.SaveChangesAsync(cancellation);
        return user;
    }

    public async Task<bool> ExistsAsync(string email, string username, CancellationToken cancellation = default)
    {
        return await _context.Users.AnyAsync(u => u.Email == email || u.Username == username, cancellation);
    }
}
