using AbySalto.Mid.Application.Auth;
using AbySalto.Mid.Application.Auth.Commands;

namespace AbySalto.Mid.Application.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterCommand command, CancellationToken cancellation = default);
    Task<AuthResponse> LoginAsync(LoginCommand command, CancellationToken cancellation = default);
    Task<UserDto?> GetCurrentUserAsync(int userId, CancellationToken cancellation = default);
}
