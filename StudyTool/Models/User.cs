using System.ComponentModel.DataAnnotations.Schema;

namespace StudyTool.Models;

[Table("User")]
public class User
{
    public int Id { get; set; }
    public bool Confirmed { get; set; } = false;
    public string DisplayName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
    public DateTime LastSeentAt { get; set; }
    public DateTime? DeletionRequestedAt { get; set; }
    public byte[] Salt { get; set; } = Array.Empty<byte>();
    public byte[] SaltedPasswordHash { get; set; } = Array.Empty<byte>();
}
