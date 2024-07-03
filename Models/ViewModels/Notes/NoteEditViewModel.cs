namespace NoteApp.Models.ViewModels.Notes;

public class NoteEditViewModel
{
    public Note Note { get; set; }
    public IList<Tag> Tags { get; set; }
    public IList<Category> Categories { get; set; }
    // TBA
    //public IList<Group>
}