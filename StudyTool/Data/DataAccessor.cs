using Microsoft.EntityFrameworkCore;
using StudyTool.BusinessLogic;
using StudyTool.Models;

namespace StudyTool.Data;

/// <summary>
/// This class provides interaction with the database by providing an instance of <see cref="DatabaseContext"/>.
/// Here properties and methods are defined to make those interactions pragmatic.
/// Make sure to use <see cref="EntityFrameworkQueryableExtensions.AsNoTracking{TEntity}(IQueryable{TEntity})"/> Method when you only read the data, as this is more efficient.
/// </summary>
public class DataAccessor
{
    private readonly ILogger<DataAccessor> _logger;
    private readonly DatabaseContext _databaseContext;
    private readonly HashManager _hashManager;

    public DataAccessor(ILogger<DataAccessor> logger, DatabaseContext databaseContext, HashManager hashManager)
    {
        _logger = logger;
        _databaseContext = databaseContext;
        _hashManager = hashManager;
    }

    public User? GetUserByMail(string mailAddress)
    {
        try
        {
            return _databaseContext
                .Users
                .AsNoTracking()
                .FirstOrDefault(u =>
                    u.EmailAddress.Equals(mailAddress)
                );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not get user by mail {mail}", mailAddress);
            return null;
        }
    }

    public bool ConfirmUserPassword(string emailAddress, string password)
    {
        var user = GetUserByMail(emailAddress);

        if (user == null)
            return false;

        return _hashManager.ConfirmMatchingHashArgon2Hash(password, user.Salt, user.SaltedPasswordHash);
    }

    public bool AddNewUser(User user)
    {
        try
        {
            _databaseContext.Users.Add(user);
            _databaseContext.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
