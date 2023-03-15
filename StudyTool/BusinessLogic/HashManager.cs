using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using StudyTool.Data;

namespace StudyTool.BusinessLogic;

/// <summary>
/// The class which manages password/salt creation & validation.
/// </summary>
public class HashManager
{
    /// <summary>
    /// The logger instance for <see cref="HashManager"/>.
    /// </summary>
    private readonly ILogger<HashManager> _logger;
    
    /// <summary>
    /// Constructor injecting logger.
    /// </summary>
    /// <param name="logger">the logger instance.</param>
    public HashManager(ILogger<HashManager> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Confirms if a hash & salt is matching the given password.
    /// </summary>
    /// <param name="password">the text to match.</param>
    /// <param name="salt">the salt</param>
    /// <param name="actualHash">the actual hash</param>
    /// <returns></returns>
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