using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NoteApp.Models;

public class User: IdentityUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(128)]
    public string Email { get; set; } = null!;
    [MaxLength(128)]
    public string Username { get; set; } = null!;
    [MaxLength(128)]
    public string FirstName { get; set; } = null!;
    [MaxLength(128)]
    public string LastName { get; set; } = null!;
    [MaxLength(64)]
    public string PasswordHash { get; set; } = null!;
    private DateTime _createdAt = DateTime.Now;
    private DateTime _updatedAt = DateTime.Now;
    
}