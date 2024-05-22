namespace NoteApp.Models;

public class Note
{
    
    private Guid _id = Guid.NewGuid();
    private string _title = null!;
    private string _body = null!;

    private enum NoteCategory
    {
        Personal,
        Work,
        School,
        Other
    };
    private DateTime _createdAt;
    private DateTime _updatedAt;

    

    
}