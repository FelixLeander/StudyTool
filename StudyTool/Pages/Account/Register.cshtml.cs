using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyTool.BusinessLogic;
using StudyTool.Data;
using StudyTool.Models;

namespace StudyTool.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly ILogger<RegisterModel> _logger;
    private readonly HashManager _hashManager;
    private readonly DataAccessor _dataAccessor;
    public RegisterModel(ILogger<RegisterModel> logger, HashManager hashManager, DataAccessor dataAccessor)
    {
        _logger = logger;
        _hashManager = hashManager;
        _dataAccessor = dataAccessor;
    }

    [BindProperty]
    public string DisplayName { get; set; } = string.Empty;

    [BindProperty]
    public string EmailAddress { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public void OnGet()
    {
    }

    //THERE ARE MANY MISSING CASES! THIS NEEDS WORK!
    public IActionResult OnPostRegister()
    {
        if (new string[] { DisplayName, EmailAddress, Password }.Any(a => string.IsNullOrWhiteSpace(a)))
            return RedirectToPage( new { MissingField = "All fields need to be filled." } );


        var salt = _hashManager.CreateRandomSalt();

        var hash = _hashManager.CreateArgon2Hash(Password, salt);
        if (hash == null)
            return RedirectToPage(new { Error = "Failed to create Argon2 hash" });

        User newUser = new()
        {
            DisplayName = this.DisplayName,
            EmailAddress = this.EmailAddress,
            RegisteredAt = DateTime.UtcNow,
            LastSeentAt = DateTime.UtcNow,
            Salt = salt,
            SaltedPasswordHash = hash
        };

        if (!_dataAccessor.AddNewUser(newUser))
            return RedirectToPage(new { Error = "Failed to add User to db" });

        return RedirectToPage("/Index");
    }
}
