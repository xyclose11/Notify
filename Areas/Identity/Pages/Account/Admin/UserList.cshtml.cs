using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NoteApp.Models;

namespace NoteApp.Areas.Identity.Pages.Account.Admin;

public class UserList : PageModel
{

    private readonly UserManager<User> _userManager;
    private readonly ILogger<UserList> _logger;
    
    public UserList(UserManager<User> userManager, ILogger<UserList> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    
    public List<User> Users { get; set; }

    // Current page
    public int CurrentPage { get; set; } = 1;
    
    // Items per page
    public int ItemsPerPage { get; set; } = 5;
    
    // Total pages
    public int TotalPages { get; set; }
    public async Task OnGetAsync(int currentPage)
    {
        CurrentPage = currentPage > 0 ? currentPage : 1;
        
        // Get total number of users
        var totalUsers = await _userManager.Users.CountAsync();
        
        // Calculate the total number of pages
        TotalPages = (int)Math.Ceiling(totalUsers / (double)ItemsPerPage);
        
        // Get the users for the current page
        Users = await _userManager.Users
            .Skip((CurrentPage - 1) * ItemsPerPage)
            .Take(ItemsPerPage)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("Successfully deleted user with ID {UserId}. Success Messages: {Errors}", userId, result.Errors);
                // Redirect to the same page to refresh
                return RedirectToPage();
            }
            else
            {
                _logger.LogError("Failed to delete user with ID {UserId}. Errors: {Errors}", userId, result.Errors);
            }
        }
        
        // If we got this far, something failed. Redisplay the page.
        return Page();
    }

    public async Task<IActionResult> OnPostAddDummyUserAsync()
    {
        var dummyUser = new User
        {
            UserName = "dummyUser",
            Email = "testUser2@example.com",
            FirstName = "John",
            LastName = "Doe",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            
        };

        var result = await _userManager.CreateAsync(dummyUser);

        if (result.Succeeded)
        {
            _logger.LogInformation("Test user created successfully.");
        }
        else
        {
            _logger.LogError("Failed to create a test user. Errors: {Errors}", result.Errors);
        }

        return RedirectToPage();
    }
    
    
    
}