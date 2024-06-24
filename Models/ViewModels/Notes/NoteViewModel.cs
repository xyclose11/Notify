namespace NoteApp.Models.ViewModels.Notes;

public class NoteViewModel
{
    public Note Note { get; set; }
    public List<Tag> UserOwnedTags { get; set; }
    public List<Tag> NonAppliedTags { get; set; }
}