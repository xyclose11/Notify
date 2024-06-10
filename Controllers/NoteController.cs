using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteApp.Helpers;


namespace NoteApp.Controllers
{
    public class NoteController : Controller
    {
        private readonly NoteDbContext _context;
        private readonly UserManager<User> _userManager;

        public NoteController(NoteDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Note
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var notes = await _context.Notes
                .Where(note => note.IsOwnedBy == userId)
                .ToListAsync();
            
            return View(notes);
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
            return View();
        }

        // POST: Note/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Body")] Note note)
        {
            note.IsOwnedBy = _userManager.GetUserId(User);
            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = DateTime.UtcNow;
            
            if (ModelState.IsValid)
            {
                _context.Add(note);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

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
    }
}