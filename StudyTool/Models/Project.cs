using System.ComponentModel.DataAnnotations.Schema;

namespace StudyTool.Models;

[Table("Project")]
public class Project
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int? ApiLogId { get; set; }
    public int? FileId { get; set; }
    public int? NoteId { get; set; }
}
