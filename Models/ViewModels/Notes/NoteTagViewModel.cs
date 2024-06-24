namespace NoteApp.Models.ViewModels.Notes;

public class NoteTagViewModel
{
    public IEnumerable<NoteApp.Models.ViewModels.Notes.NoteViewModel> NoteViewModels { get; set; }
    public IEnumerable<NoteApp.Models.Tag> Tags { get; set; }
}