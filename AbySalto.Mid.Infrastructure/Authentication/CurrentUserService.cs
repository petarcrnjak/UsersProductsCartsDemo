using AbySalto.Mid.Application.Auth;
using AbySalto.Mid.Application.Auth.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AbySalto.Mid.Infrastructure.Authentication;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public int GetUserId()
    {
        var id = TryGetUserId();
        if (id is null)
            throw new UnauthorizedAccessException("User not authenticated");
        return id.Value;
    }

    public int? TryGetUserId()
    {
        var value = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(value) || !int.TryParse(value, out var id))
            return null;
        return id;
    }

    public async Task<string?> GetUsernameAsync()
    {
        if (!IsAuthenticated)
            return null;

        var identityName = User?.Identity?.Name;
        if (!string.IsNullOrWhiteSpace(identityName))
            return identityName.Trim();

        var id = TryGetUserId();
        if (id is null)
            return null;

        var user = await _userRepository.GetByIdAsync(id.Value);
        return string.IsNullOrWhiteSpace(user?.Username) ? null : user.Username.Trim();
    }
}
