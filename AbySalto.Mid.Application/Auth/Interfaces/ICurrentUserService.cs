
namespace AbySalto.Mid.Application.Auth.Interfaces;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    int GetUserId();
    Task<string?> GetUsernameAsync();
    int? TryGetUserId();
}
