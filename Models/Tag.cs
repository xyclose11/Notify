using System.ComponentModel.DataAnnotations;

namespace NoteApp.Models;

public class Tag
{
    [Required]
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    public IList<NoteTag> NoteTags { get; set; } = new List<NoteTag>();
    
    private string _name;

    [Required] [MaxLength(128)] public string Name
    {
        get => _name;
        set
        {
            _name = value;
            Slug = value.ToLower(); // Ensures that when the Name is created it asserts slug as well
        }
    }
    [MaxLength(128)] public string Slug { get; set; } = null!;// Lowercase version of Name
    public string? Description { get; set; }
    
    [Required] public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    [Required] public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    [Required] public string? WasCreatedBy { get; set; }
    
    public string? Color { get; set; } = "#000000"; // Default color is black
    public int? NoteCount { get; set; } = 0; // Contains amount of notes associated with the tag
}