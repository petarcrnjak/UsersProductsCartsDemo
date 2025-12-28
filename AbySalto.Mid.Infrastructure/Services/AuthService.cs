using AbySalto.Mid.Application.Auth;
using AbySalto.Mid.Application.Auth.Authentification;
using AbySalto.Mid.Application.Auth.Commands;
using AbySalto.Mid.Application.Auth.Interfaces;
using AbySalto.Mid.Application.Common.Exceptions;
using AbySalto.Mid.Application.Services;
using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Infrastructure.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenProvider tokenProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
    }

    public async Task<UserDto?> GetCurrentUserAsync(int userId, CancellationToken cancellation = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellation);
        if (user is null)
            return null;

        return new UserDto(user.Username, user.Email, user.CreatedAt);
    }

    public async Task<AuthResponse> LoginAsync(LoginCommand command, CancellationToken cancellation = default)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, cancellation);
        if (user is null || !_passwordHasher.VerifyPassword(command.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        var token = _tokenProvider.Generate(user);
        return new AuthResponse(user.Username, user.Email, token);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterCommand command, CancellationToken cancellation = default)
    {
        if (await _userRepository.ExistsAsync(command.Email, command.Username, cancellation))
        {
            throw new ConflictException("User with this email or username already exists.");
        }

        var passwordHash = _passwordHasher.HashPassword(command.Password);
        var user = new User(command.Username, command.Email, passwordHash, DateTime.UtcNow);

        var created = await _userRepository.AddAsync(user, cancellation);
        var token = _tokenProvider.Generate(created);

        return new AuthResponse(created.Username, created.Email, token);
    }
}
