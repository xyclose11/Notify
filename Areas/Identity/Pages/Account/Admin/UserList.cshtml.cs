using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NoteApp.Models;

namespace NoteApp.Areas.Identity.Pages.Account.Admin;

public class UserList : PageModel
{

    private readonly UserManager<User> _userManager;
    
    public void OnGet()
    {
        
    }
    
    public UserList(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public List<User> Users { get; set; }

    public async Task OnGetAsync()
    {
        Users = await _userManager.Users.ToListAsync();
    }
}