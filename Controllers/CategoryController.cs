using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteApp.Helpers;
using NoteApp.Models;

namespace NoteApp.Controllers;

public class CategoryController: Controller
{
    private readonly NoteDbContext _context;

    public CategoryController(NoteDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(Category category)
    {

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
        if (ModelState.IsValid)
        {
            
            _context.Categories.Add(category);
            _context.SaveChanges();
            
            // Redirect to previous page
            return RedirectToAction("Index", "Note");
            
        }
        return View(category);
    }
    
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _context.Categories.ToListAsync();
        return Ok(categories);
    }
}