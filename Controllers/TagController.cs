using System.Text.RegularExpressions;
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
    public async Task<IActionResult> Create(string Name, string Description, string Color)
    {
        // Removed from the ModelState since it is required on the DB side but cannot be added until after it is created
        ModelState.Remove("WasCreatedBy");
        
        // SSValidation
        if (string.IsNullOrEmpty(Name) || Name.Length > 128)
        {
            ModelState.AddModelError(nameof(Name), "Name is required and/or must be under 128 characters.");
            return RedirectToAction("Index", "Note");
        }
        if (Description.Length > 256)
        {
            ModelState.AddModelError("Description", "Description length must be under 128 characters.");
        }
        if (string.IsNullOrEmpty(Color) || !Regex.IsMatch(Color, "^#(?:[0-9a-fA-F]{3}){1,2}$"))
        {
            ModelState.AddModelError("Color", "Color is required and/or must be a valid hex color.");
        }
        
        var tag = new Tag
        {
            Name = Name,
            Description = Description,
            Color = Color,
            WasCreatedBy = _userManager.GetUserId(User)
        };
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Tag: <strong> {tag.Name} </strong> created successfully!";

        return RedirectToAction("Index", "Note");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid tagId)
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

        var tag = await _context.Tags.FindAsync(tagId);

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
        
        TempData["SuccessMessage"] = $"Tag: <strong> {tag.Name} </strong> deleted successfully!";

        return RedirectToAction("Index", "Note");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid tagId, string name, string description, string color)
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

        var tag = await _context.Tags.FindAsync(tagId);

        if (tag != null)
        {
            // Update tag
            tag.Name = name;
            tag.Description = description;
            tag.Color = color;
            tag.UpdatedAt = DateTime.UtcNow;
        }
        
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Changes on tag: <strong> {tag.Name} </strong> saved successfully!";
        return RedirectToAction("Index", "Note");
    }
    
}