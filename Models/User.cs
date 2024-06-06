using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NoteApp.Models;

public class User: IdentityUser
{
    [MaxLength(128)]
    [PersonalData] // Deleted when user deletes account
    public string FirstName { get; set; } = null!;
    [MaxLength(128)]
    [PersonalData] // Deleted when user deletes account
    public string LastName { get; set; } = null!;
    [MaxLength(64)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    
    
    
}