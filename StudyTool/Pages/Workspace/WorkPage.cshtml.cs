using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Aspose.Slides;
using System.IO;
using StudyTool.BusinessLogic;

namespace StudyTool.Pages.Workspace;

public class WorkPageModel : PageModel
{
    public string _apiData = "{PlaceHolder}";
    public List<string>? _slides;
    public string _noteData = "{PlaceHolder}";

    private readonly ILogger<WorkPageModel> _logger;
    private readonly IWebHostEnvironment _environment;
    public WorkPageModel(ILogger<WorkPageModel> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public IActionResult OnGet(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return RedirectToPage("/Common/Error", new { errorMessage = "You need to select a file." });

        var filePath = Path.Combine(_environment.ContentRootPath, "uploads", fileName);

        if (!System.IO.File.Exists(filePath))
            return RedirectToPage("/Common/Error", new {errorMessage = "Cant find file."});

        _slides = PresentationConversion.ConvertToSvgSlides(filePath);
        if (_slides == null)
            return RedirectToPage("/Common/Error", new { errorMessage = "Cant create slides." });

        return Page();
    }
}