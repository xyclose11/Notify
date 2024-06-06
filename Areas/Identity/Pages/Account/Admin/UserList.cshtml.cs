using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NoteApp.Models;

namespace NoteApp.Areas.Identity.Pages.Account.Admin;

public class UserList : PageModel
{

    private readonly UserManager<User> _userManager;
    

    
    public UserList(UserManager<User> userManager)
    {
        _userManager = userManager;
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
}