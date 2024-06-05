using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoteApp.Models;

namespace NoteApp.Areas.Identity.Pages.Account.Admin;

[Authorize(Roles = "Admin")]
public class Index : PageModel
{
    private readonly UserManager<User> _userManager;

    public void OnGet()
    {
        
    }

    public Index(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    

}