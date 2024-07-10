namespace NoteApp.Models.ViewModels.Notes;

public class TagCategoryViewModel
{
    public IEnumerable<NoteApp.Models.Tag> Tags { get; set; }
    public List<NoteApp.Models.Category> Categories { get; set; }
}