using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace StudyTool.Pages;

public class IndexModel : PageModel
{
    private readonly IWebHostEnvironment _environment;
    public IndexModel(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [BindProperty]
    public IFormFile? Upload { get; set; }
    public async Task<IActionResult> OnPostAsync()
    {
        if (Upload == null)
            return Page();

        var file = Path.Combine(_environment.ContentRootPath, "uploads", Upload.FileName);
        using (var fileStream = new FileStream(file, FileMode.Create))
        {
            await Upload.CopyToAsync(fileStream);
        }

        return RedirectToPage("/Workspace/WorkPage", new { Upload.FileName });
    }
}
