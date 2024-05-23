using System.ComponentModel.DataAnnotations;

namespace NoteApp.Models;

public class Note
{
    
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(128)]
    public string Title { get; set; } = null!;
    public string Body = null!;

    private enum NoteCategory
    {
        Personal,
        Work,
        School,
        Other
    };
    private DateTime _createdAt;
    private DateTime _updatedAt;
}