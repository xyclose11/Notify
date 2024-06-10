using System.ComponentModel.DataAnnotations;

namespace NoteApp.Models;

public class Note
{
    
    public Guid Id { get; init; } = Guid.NewGuid();
    [MaxLength(128)]
    public string Title { get; set; } = null!;
    [MaxLength(2048)]
    public string Body { get; set; } = null!;

    public string? IsOwnedBy { get; set; }

    private enum NoteCategory
    {
        Personal,
        Work,
        School,
        Other
    };
    
    [Required]
    public DateTime CreatedAt;

    [Required]
    public DateTime UpdatedAt;
}