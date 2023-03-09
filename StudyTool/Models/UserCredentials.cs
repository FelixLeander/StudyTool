namespace StudyTool.Models;

public class UserCredentials
{
    /// <summary>
    /// Gets or sets the user name.
    /// </summary>
    public string EmailAddress { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public string Password { get; init; } = string.Empty;
}