using System.ComponentModel.DataAnnotations;

namespace NoteApp.Models;

public class Category
{
    [Key]
    public Guid CategoryId { get; init; } = Guid.NewGuid();
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? WhoCreated { get; set; }
    public bool isPublic { get; set; } = false;
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}