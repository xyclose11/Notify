using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NoteApp.Areas.Identity.Pages.Account.Admin;

[Authorize(Roles = "Admin")]
public class _Layout : PageModel
{
    public void OnGet()
    {
        
    }
}