using System.Security.Cryptography;

namespace EventPlatform.Utilities;

/// <summary>
/// Handles password hashing and verification using BCrypt.
/// </summary>
public static class PasswordHelper
{
    private const int SaltSize = 16;         //128-bit salt
    private const int HashSize = 32;         //256-bit hash
    private const int Iterations = 100_000;  // NIST recommended minimum

    /// <summary>Hashes a plain-text password.</summary>
    public static string Hash(string password)
    {
        //1.Generate a random salt
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        
        //2.Hash the password using PBKDF2 with SHA-256
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize
        );
        
        //3.Store as "iterations:salt:hash" so Verify can use the same values
        return $"{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    /// <summary>Verifies a plain-text password against a stored hash.</summary>
    public static bool Verify(string password, string storedHash)
    {
        //1.Split the stored string back into its parts
        string[] parts = storedHash.Split(':');
        if (parts.Length != 3) return false;

        int iterations = int.Parse(parts[0]);
        byte[] salt = Convert.FromBase64String(parts[1]);
        byte[] hash = Convert.FromBase64String(parts[2]);
        
        //2.Hash the incoming password with the same salt and iterations
        byte[] attemptHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            HashSize
        );
        
        //3.Compare securely (prevents timing attacks)
        return CryptographicOperations.FixedTimeEquals(hash, attemptHash);
    }
}
