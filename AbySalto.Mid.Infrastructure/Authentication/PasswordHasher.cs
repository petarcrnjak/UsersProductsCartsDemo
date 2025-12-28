using AbySalto.Mid.Application.Auth.Authentification;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace AbySalto.Mid.Infrastructure.Authentication;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 128 / 8;
    private const int HashSize = 256 / 8;
    private const int Iterations = 10000;

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: Iterations,
            numBytesRequested: HashSize
        );

        var combined = new byte[SaltSize + HashSize];
        Buffer.BlockCopy(salt, 0, combined, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, combined, SaltSize, HashSize);

        return Convert.ToBase64String(combined);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        var combined = Convert.FromBase64String(passwordHash);
        var salt = new byte[SaltSize];
        var storedHash = new byte[HashSize];

        Buffer.BlockCopy(combined, 0, salt, 0, SaltSize);
        Buffer.BlockCopy(combined, SaltSize, storedHash, 0, HashSize);

        var hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: Iterations,
            numBytesRequested: HashSize
        );

        return CryptographicOperations.FixedTimeEquals(hash, storedHash);
    }
}
