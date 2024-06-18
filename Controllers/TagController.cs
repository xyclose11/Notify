using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteApp.Helpers;
using NoteApp.Models;


namespace NoteApp.Controllers;

public class TagController: Controller
{
    private readonly NoteDbContext _context;
    private readonly UserManager<User> _userManager;

    public TagController(NoteDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(Tag tag)
    {
        ModelState.Remove("WasCreatedBy");
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            foreach (var error in errors)
            {
                Console.WriteLine($"Key: {error.Key}");
                foreach (var modelError in error.Errors)
                {
                    Console.WriteLine($"Error: {modelError.ErrorMessage}");
                }
            }
        }

        if (!ModelState.IsValid) return View(tag);
        tag.WasCreatedBy = _userManager.GetUserId(User);
        _context.Tags.Add(tag);
        _context.SaveChanges();
            
        // Redirect to previous page
        return RedirectToAction("Index", "Note");
    }
}