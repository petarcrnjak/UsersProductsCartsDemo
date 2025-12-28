namespace AbySalto.Mid.Application.Auth.Interfaces;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    int GetUserId();
    int? TryGetUserId();
}
