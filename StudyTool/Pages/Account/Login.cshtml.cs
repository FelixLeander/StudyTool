using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyTool.BusinessLogic;
using StudyTool.Data;
using StudyTool.Models;
using System.Security.Claims;
using System.Runtime.CompilerServices;
using Aspose.Slides.MathText;

namespace StudyTool.Pages.Account;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly DataAccessor _userAccessor;
    private readonly HashManager _hashManager;

    internal string? _errorMessage;
    public LoginModel(ILogger<LoginModel> logger, DataAccessor userAccessor, HashManager hashManager)
    {
        _logger = logger;
        _userAccessor = userAccessor;
        _hashManager = hashManager;
    }

    /// <summary>
    /// Gets or sets the user inputs.
    /// </summary>
    [BindProperty]
    public UserCredentials Input { get; set; } = new();

    public void OnGetAsync(string? errorMessage = null)
        => _errorMessage = errorMessage;

    public async Task<IActionResult> OnPostLogin(string? emailAddress = null, string? passwordHash = null)
    {
        if (!(string.IsNullOrWhiteSpace(emailAddress) || string.IsNullOrWhiteSpace(passwordHash)))
        {
            Input = new UserCredentials()
            {
                EmailAddress = emailAddress,
                Password = passwordHash
            };
        }

        var user = _userAccessor.GetUserByMail(Input.EmailAddress);
        if (user == null || !_hashManager.ConfirmMatchingHashArgon2Hash(Input.Password, user.Salt, user.SaltedPasswordHash))
            return RedirectToPage(new { errorMessage = "Bad Email or Password!" });

        try
        {
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = false,
            };

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.DisplayName),
                new(ClaimTypes.Role, "RegistredUser")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToPage("/Account/Profile");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignIn failed for email input {input}", Input.EmailAddress);
            return RedirectToPage(new { errorMessage = "Error logging in!" });
        }

    }
}
