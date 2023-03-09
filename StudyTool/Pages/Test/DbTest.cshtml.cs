using Aspose.Slides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyTool.BusinessLogic;
using StudyTool.Data;
using StudyTool.Models;

namespace StudyTool.Pages.Test;

/// <summary>
/// THIS PAGE IS FOR TESTING ONLY AND SHOULD NOT BE EXSISTING IN PRODUCTION!
/// </summary>
public class DbTestModel : PageModel
{
    private readonly DataAccessor _dataAccessor;
    private readonly IWebHostEnvironment _env;
    private readonly HashManager _hashManager;
    public DbTestModel(DataAccessor dataAccessor, IWebHostEnvironment env, HashManager hashManager)
    {
        _dataAccessor = dataAccessor;
        _env = env;
        _hashManager = hashManager;
    }

    public void OnGet()
    {
        if (_env.IsProduction() || _env.IsStaging())
            throw new ApplicationException("Remove testing & debugging paged before staging/production!");
    }

    public IActionResult OnCreateUser(string userName, string userMail, string userPassword)
    {
        var salt = _hashManager.CreateRandomSalt();
        if (salt == null && _env.IsDevelopment())
            throw new NullReferenceException("THIS SHOULD BE REFACTORED");
        if (salt == null)
            return RedirectToPage("/Common/Error", new { errorMessage = "Could not create salt for hashing." });

        var saltedHash = _hashManager.CreateArgon2Hash(userPassword, salt);
        if (saltedHash == null && _env.IsDevelopment())
            throw new NullReferenceException("THIS SHOULD BE REFACTORED");
        if (saltedHash == null)
            return RedirectToPage("/Common/Error", new { errorMessage = "Could not create salted hash for password." });

        var currentTime = DateTime.Now;
        var newUser = new User()
        {
            //ToDo: Check if db does increment Id or if does need to be done manually 
            DisplayName = userName,
            EmailAddress = userMail,
            RegisteredAt = currentTime,
            LastSeentAt = currentTime,
            Salt = salt,
            SaltedPasswordHash = saltedHash
        };

        _dataAccessor.AddNewUser(newUser);

        return RedirectToPage("/");
    }
}
