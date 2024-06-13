using System.ComponentModel.DataAnnotations;

namespace NoteApp.Models;

public class NoteCategory
{
    [Key]
    public Guid CategoryId { get; init; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Description { get; set; }
}