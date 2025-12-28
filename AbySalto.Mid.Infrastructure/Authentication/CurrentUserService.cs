using AbySalto.Mid.Application.Auth.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AbySalto.Mid.Infrastructure.Authentication;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
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
}
