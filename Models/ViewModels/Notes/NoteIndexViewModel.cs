using System.Collections.Generic;
using NoteApp.Models;

namespace NoteApp.Models.ViewModels.Notes;

public class NoteIndexViewModel
{
    public List<Note> Notes { get; set; }
    public IList<string> Roles { get; set; }
}