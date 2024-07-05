using System.Collections;
using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoteApp.Helpers;
using NoteApp.Models.ViewModels.Notes;


namespace NoteApp.Controllers
{
    public class NoteController : Controller
    {
        private readonly NoteDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<NoteController> _logger;

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 20;
        public string SelectedCategory { get; set; } = null!;
        public List<Guid> SelectedTags { get; set; } = null!;
        
        public NoteController(NoteDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<NoteController> iLogger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = iLogger;
        }

        // GET: Note
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            
            var notes = await _context.Notes
                .Where(note => note.IsOwnedBy == userId)
                .ToListAsync();
                
            var model = new NoteIndexViewModel
            {
                Notes = notes,
                Roles = roles
            };
            
            return View(model);
        }

        // GET: Note/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = _context.Notes.FirstOrDefault(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Note/Create
        public IActionResult Create()
        {
            // Pass all the categories to the view in a dropdown list
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewBag.UserOwnedTags = new SelectList(_context.NoteTags
                .Where(tag => tag.Tag.WasCreatedBy == _userManager.GetUserId(User)), "Id", "Name");
            return View();
        }

        // POST: Note/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Body,CategoryId,NoteTags")] Note note, List<Guid> selectedTags)
        {
            note.IsOwnedBy = _userManager.GetUserId(User);
            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                // Check if selectedTags is null or empty
                if (selectedTags == null || !selectedTags.Any())
                {
                    _logger.LogInformation("No tags were selected.");
                }
                else
                {
                    // Add any selected tags to note
                    foreach (var Id in selectedTags)
                    {
                        var tag = _context.Tags.Find(Id);
                        if (tag != null)
                        {
                            note.NoteTags.Add(new NoteTag { Note = note, Tag = tag});
                        }
                        else
                        {
                            _logger.LogInformation("No tag found with ID: {Id}", Id);
                        }
                    }
                }

                _context.Notes.Add(note);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            // If the model state is not valid, repopulate the categories and return the view
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewBag.UserOwnedTags = new SelectList(_context.NoteTags
                .Where(tag => tag.Tag.WasCreatedBy == _userManager.GetUserId(User)), "Id", "Name");

            return View(note);
        }

        private NoteEditViewModel GetNoteEditViewModel(Note note)
        {
            var userId = _userManager.GetUserId(User);
            // Get categories
            var categories = _context.Categories
                .Where(cat => cat.WhoCreated == userId)
                .ToList();

            // Get tags
            var tags = _context.Tags
                .Where(tag => tag.WasCreatedBy == userId)
                .ToList();
            
            // Get tags that are applied to the note
            var appliedTags = _context.NoteTags
                .Where(nt => nt.NoteId == note.Id)
                .Select(appliedTag => appliedTag.Tag)
                .ToList();


            var noteEditModel = new NoteEditViewModel
            {
                Note = note,
                Categories = categories,
                Tags = tags,
                AppliedTags = appliedTags,
            };

            return noteEditModel;
        }
        // GET: Note/Edit/5
        [HttpGet]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = _context.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }

            var noteEditViewModel = GetNoteEditViewModel(note);
            return View(noteEditViewModel);
        }

        // POST: Note/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Title,Body")] Note note)
        {
            if (!ModelState.IsValid) return View(GetNoteEditViewModel(note));
            var updatedNote = await _context.FindAsync<Note>(note.Id);

            // Update each binded field
            if (note.Title != null)
            {
                updatedNote.Title = note.Title;
            }

            if (note.Body != null)
            {
                updatedNote.Body = note.Body;
            }

            if (note.CategoryId != null)
            {
                updatedNote.CategoryId = note.CategoryId;
            }
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //TODO ADD GROUP FEATURE TO THIS
        [HttpPost]
        public async Task<IActionResult> EditActions(Guid noteId, Guid? categoryId, List<Guid>? tagIds, bool? isPublic)
        {
            // Validate noteId
            var updatedNote = await _context.Notes.FindAsync(noteId);
            
            if (updatedNote == null) return NotFound();
            
            // Validate category
            if (categoryId != null)
            {
                var newCategory = await _context.Categories.FindAsync(categoryId);
                updatedNote.Category = newCategory;
                updatedNote.CategoryId = categoryId;
            }
            

            if (tagIds.Count > 0)
            {
                // Keep already applied tags
                // Apply new tags
                
            }
            

            // TODO Change isPublic to regular setter
            // if (isPublic != null)
            // {
            //     updatedNote.IsPublic = isPublic;
            // }

            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        // POST: Note/Delete/5
        [HttpGet]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var note = _context.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }
        
        // POST: /Note/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var note = _context.Notes.Find(id);
            _context.Notes.Remove(note);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        
        // POST: /Note/DeleteMany
        [HttpGet]
        public IActionResult DeleteMany(List<string> noteIds)
        {
            var noteList = new List<Note> { };
            foreach (var id in noteIds)
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var note = _context.Notes.Find(id);
                if (note == null)
                {
                    return NotFound();
                }
                
                noteList.Add(note);
            }


            return PartialView("DeleteManyPartial/_DeleteMany", noteList);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteManyConfirmation(List<Guid> noteIds)
        {
            if (noteIds.Count == 0)
            {
                TempData["ErrorMessage"] = $"No notes selected";
                return BadRequest();
            }
            try
            {
                foreach (var id in noteIds)
                {
                    var note = await _context.Notes.FindAsync(id);
                    if (note == null)
                    {
                        Console.WriteLine("Note does not exist");
                        return NotFound();
                    }
                    _context.Notes.Remove(note);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
  
            TempData["SuccessMessage"] = $"Notes were deleted.";           
            return RedirectToAction("Index");
        }
        private IEnumerable<NoteViewModel> GetNoteViewModels(string userId, string category, int currentPage, List<Guid> selectedTags)
        {

            // Get notes from DB
            var tableNotesQuery = _context.Notes
                .Include(note => note.Category)
                .Include(note => note.NoteTags)
                .ThenInclude(nt => nt.Tag)
                .Where(note => note.IsOwnedBy == userId);

            if (!string.IsNullOrEmpty(category))
            {
                tableNotesQuery = tableNotesQuery.Where(note => note.Category.Name == category);
            }

            // Filter notes based on selected tags
            if (selectedTags is { Count: > 0 })
            {
                tableNotesQuery = tableNotesQuery.Where(note => note.NoteTags.Any(nt => selectedTags.Contains(nt.TagId)));
            }
            
            // Pagination
            var tableNotes = tableNotesQuery
                .Skip((CurrentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage)
                .ToList();
            
            // Map the notes to NoteViewModels
            var noteViewModels = tableNotes.Select(note =>
            {
                // Get the tags created by the owner of the note
                var userOwnedTags = _context.Tags.Where(tag => tag.WasCreatedBy == note.IsOwnedBy).ToList();

                // Get the tags that are not already applied to the note
                var nonAppliedTags = userOwnedTags.Where(tag => !note.NoteTags.Select(nt => nt.TagId).Contains(tag.Id)).ToList();

                return new NoteViewModel
                {
                    Note = note,
                    UserOwnedTags = userOwnedTags,
                    NonAppliedTags = nonAppliedTags
                };
            });

            return noteViewModels;
        }
        
        private IEnumerable<Tag> GetUserOwnedTags(string userId)
        {
            // Get the tags from the database
            var tags = _context.Tags
                .Where(tag => tag.WasCreatedBy == userId)
                .ToList();

            return tags;
        }

        public async Task<IActionResult> GetNotes(string view, string category, int currentPage, List<Guid> filterTags)
        {
            var userId = _userManager.GetUserId(User);
            
            if (view == "Table")
            {
                CurrentPage = currentPage > 0 ? currentPage : 1;
                SelectedCategory = category;
                SelectedTags = filterTags;
                
                var totalNotesForUser = await _context.Notes
                    .Where(note => note.IsOwnedBy == userId)
                    .Where(note => note.Category.Name == SelectedCategory)
                    .CountAsync();

                TotalPages = (int)Math.Ceiling(totalNotesForUser / (double)ItemsPerPage);

                ViewBag.CurrentPage = CurrentPage;
                ViewBag.TotalPages = TotalPages;
                ViewBag.Categories = await _context.Categories.ToListAsync();
     
                var noteTagViewModels = new NoteTagViewModel
                {
                    NoteViewModels = GetNoteViewModels(userId, category, currentPage, filterTags),
                    Tags = GetUserOwnedTags(userId)
                };

                return PartialView("_TableView", noteTagViewModels);
            }

            
            var cardNotes = _context.Notes
                .Where(note => note.IsOwnedBy == userId)
                .ToList();

            return PartialView("_CardView", cardNotes);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTestNotes()
        {
            
            List<Note> noteList = new List<Note>();
 
            List<string> titles = [
                "This is an example of a title.",
                "Today a goose went to the pond.",
                "Doctors appointment on the 14th of July 2024. 07/14/2024",
                "There are many variations of plants in the garden at the college.",
                "The sky seemed to be more green than blue today.",
                "Asparagus, Leeks, Ground Beef, Tomatoes...",
                "Dentist @ 5:00 pm CST",
                "Reminder to research into the world of trees.",
                "School starts on the 21 of August",
                "Daily note!",
                "Reminder that birds are fake."
            ];
            
            List<string> bodyEntries =
            [
                "Today I learned about the importance of clean code. It's not just about making the code work, it's also about making it understandable and maintainable.",
                "I need to remember to buy groceries after work. The list includes milk, bread, eggs, and some fresh fruits.",
                "I had a great idea for a new feature in our app. I think adding a dark mode would really enhance the user experience.",
                "I had a meeting with the team today. We discussed the progress of our current project and planned the tasks for next week.",
                "I read an interesting article about machine learning. It's amazing how it's being used in so many different fields.",
                "I watched a movie called 'The Imitation Game' today. It's about Alan Turing and his contributions to computer science. Highly recommended!",
                "I need to schedule a dentist appointment. I should do it first thing in the morning.",
                "I started reading a new book. It's called 'Clean Code' by Robert C. Martin. So far, it's very informative and helpful.",
                "I went for a run in the park today. The weather was perfect and I managed to run 5km. I feel great!",
                "I need to research more about design patterns. They seem to be very useful for writing better and more efficient code."
            ];
            
            // Loop iterator
            var randIterator = RandomNumberGenerator.GetInt32(0, 10);

            // Set to remember what title/body values were used before
            HashSet<int> usedNoteValues = new HashSet<int>();
            
            
            for (var i = 0; i < randIterator; i++)
            {
                var newRand = RandomNumberGenerator.GetInt32(0, 10);

                while (usedNoteValues.Contains(newRand))
                {
                    // Generate new index until uniqueness is reached
                    newRand = RandomNumberGenerator.GetInt32(0, 10);
                }
                
                // Add index to set
                usedNoteValues.Add(newRand);
                
                noteList.Add(new Note
                {
                    Id = new Guid(),
                    Title = titles[newRand],
                    Body = bodyEntries[newRand],
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsOwnedBy = _userManager.GetUserId(User),
                });
            }

            if (!ModelState.IsValid) return BadRequest("Note creation failed due to invalid model state.");
            
            // Save each note to the DB
            foreach (var note in noteList)
            {
                _context.Add(note);
            }
            await _context.SaveChangesAsync();
            
            // Redirect to the page
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> AddTagToNote(List<Guid> selectedTags, Guid noteId)
        {
            var note = await _context.Notes.FindAsync(noteId);

            if (note == null)
            {
                return NotFound();
            }
            foreach (var Id in selectedTags)
            {
                var tag = await _context.Tags.FindAsync(Id);
                if (tag == null)
                {
                    TempData["ErrorMessage"] = $"{tag.Name} Was not found.";           
                    ModelState.AddModelError("Name", "Tag Name not found.");
                }

                if (note.NoteTags.All(nt => nt.TagId != Id))
                {
                    tag.NoteTags.Add(new NoteTag{ Note = note, Tag = tag});
                    
                    // Update tag noteCount by 1
                    tag.NoteCount += 1;
                }
                else
                {
                    TempData["ErrorMessage"] = $"{tag.Name} Already Exists.";           

                    ModelState.AddModelError("Name", "Tag already exists.");
                }
            }
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Tag(s) added to {note.Title} successfully";           
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveTagFromNote(Guid selectedTagId, Guid noteId)
        {

            var noteTag = await _context.NoteTags
                .Where(nt => nt.NoteId == noteId && nt.TagId == selectedTagId)
                .FirstOrDefaultAsync();

            if (noteTag != null)
            {
                _context.NoteTags.Remove(noteTag);
                
                // Decrement Tag NoteCount field by 1
                var tag = await _context.Tags.FindAsync(selectedTagId);
                if (tag is { NoteCount: >= 1 })
                {
                    // Decrease the NoteCount by 1
                    tag.NoteCount -= 1;
                }
                
                TempData["SuccessMessage"] = $"Tag(s): <strong> {tag.Name} </strong> removed successfully";           

            }
            else
            {
                TempData["ErrorMessage"] = "ERROR: NoteTag is null.";           
                ModelState.AddModelError("Name", "Tag already exists.");
            }

            
            
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        
        
        
    }
    
}