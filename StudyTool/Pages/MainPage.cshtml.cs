using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Aspose.Slides;
using System.IO;

namespace StudyTool.Pages;

public class MainPageModel : PageModel
{
    public string? _presentation;
    private readonly ILogger<MainPageModel> _logger;
    private readonly IWebHostEnvironment _environment;

    public MainPageModel(ILogger<MainPageModel> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public void OnGet(string fileName)
    {
        var filePath = Path.Combine(_environment.ContentRootPath, "uploads", fileName);

        _presentation = System.IO.File.Exists(filePath) ? filePath : null;
    }
}