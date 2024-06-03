using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using System.Linq;
using NoteApp.Helpers;

namespace NoteApp.Controllers
{
    public class NoteController : Controller
    {
        private readonly NoteDbContext _context;

        public NoteController(NoteDbContext context)
        {
            _context = context;
        }

        // GET: Note
        public IActionResult Index()
        {
            return View(_context.Notes.ToList());
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