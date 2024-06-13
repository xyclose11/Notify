using System.Collections;
using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using System.Linq;
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

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 10;
        
        public NoteController(NoteDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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
            return View();
        }

        // POST: Note/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Body,CategoryId")] Note note)
        {
            note.IsOwnedBy = _userManager.GetUserId(User);
            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = DateTime.UtcNow;

            note.Category.CreatedAt = DateTime.UtcNow;
            note.Category.UpdatedAt = DateTime.UtcNow;
            note.Category.WhoCreated = _userManager.GetUserId(User);
            
            if (ModelState.IsValid)
            {
                _context.Notes.Add(note);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            // If the model state is not valid, repopulate the categories and return the view
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name");
            
            return View(note);
        }

        // GET: Note/Edit/5
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
            return View(note);
        }

        // POST: Note/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Title,Body")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(note);
            _context.Update(note);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        

        // POST: Note/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var note = _context.Notes.Find(id);
                if (note == null)
                {
                    return Json(new { success = false });
                }

                _context.Notes.Remove(note);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

         
        public async Task<IActionResult> GetNotes(string view, int currentPage, string sortOption)
        {
            var userId = _userManager.GetUserId(User);
            
            if (view == "Table")
            {
                CurrentPage = currentPage > 0 ? currentPage : 1;

                
                var totalNotesForUser = await _context.Notes.Where(note => note.IsOwnedBy == userId).CountAsync();

                TotalPages = (int)Math.Ceiling(totalNotesForUser / (double)ItemsPerPage);

                ViewBag.CurrentPage = CurrentPage;
                ViewBag.TotalPages = TotalPages;
                ViewBag.Categories = _context.Categories.ToListAsync();

                var tableNotesQuery = _context.Notes
                    .Include(note => note.Category)
                    .Where(note => note.IsOwnedBy == userId)
                    .Skip((CurrentPage - 1) * ItemsPerPage)
                    .Take(ItemsPerPage);

                switch (sortOption)
                {
                    case "dateCreated":
                        tableNotesQuery = tableNotesQuery.OrderBy(note => note.CreatedAt);
                        break;
                    case "dateUpdated":
                        tableNotesQuery = tableNotesQuery.OrderBy(note => note.UpdatedAt);
                        break;
                    case "title":
                        tableNotesQuery = tableNotesQuery.OrderBy(note => note.Title);
                        break;
                    case "body":
                        tableNotesQuery = tableNotesQuery.OrderBy(note => note.Body);
                        break;
                        
                }

                var tableNotes = await tableNotesQuery.ToListAsync();

                return PartialView("_TableView", tableNotes);
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
        
        
        
    }
}