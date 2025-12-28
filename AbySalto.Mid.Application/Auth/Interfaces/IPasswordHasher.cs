namespace AbySalto.Mid.Application.Auth.Authentification;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
