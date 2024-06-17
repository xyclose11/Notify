using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteApp.Models;

public class Note
{
    
    public Guid Id { get; init; } = Guid.NewGuid();
    [MaxLength(128)]
    public string Title { get; set; } = null!;
    [MaxLength(2048)]
    public string Body { get; set; } = null!;

    public string? IsOwnedBy { get; set; }
    
    [Required] 
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }
    
    public Guid? CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    public List<Tag> Tags { get; } = [];
    
    [Required]
    public bool IsPublic { get; init; } = false;
}