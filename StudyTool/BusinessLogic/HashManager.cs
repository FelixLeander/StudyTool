using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using StudyTool.Data;

namespace StudyTool.BusinessLogic;

public class HashManager
{
    private readonly ILogger<HashManager> _logger;
    public HashManager(ILogger<HashManager> logger)
    {
        _logger = logger;
    }

    internal bool ConfirmMatchingHashArgon2Hash(string password, byte[] salt, byte[] actualHash)
    {
        try
        {
            var hash = CreateArgon2Hash(password, salt);
            return hash != null && actualHash.SequenceEqual(hash);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Could not confirm salted password. {salt}.", salt);
            return false;
        }
    }

    internal byte[]? CreateArgon2Hash(string password, byte[] salt)
    {
        try
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8, // four cores
                Iterations = 4,
                MemorySize = 512 * 512 // ca. 0.25 GB
            };

            return argon2.GetBytes(16);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Could not create hash with salt {salt}.", salt);
            return null;
        }
    }

    internal byte[] CreateRandomSalt(int byteSize = 16)
    {
        var salt = new byte[byteSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        return salt;
    }
}