using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace StudyTool.Pages.Common;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string DisplayMessage { get; set; } = "No Error found.";
    private readonly ILogger<ErrorModel> _logger;
    public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }

    public void OnGet(string errorMessage)
    {
        if (!string.IsNullOrWhiteSpace(errorMessage))
            DisplayMessage = errorMessage;
    }
}