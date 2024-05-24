using NoteApp.Helpers;

namespace NoteApp.Models.Helpers;

public static class DbInit
{
    public static void Initialize(NoteDbContext context)
    {
        if (context.Notes.Any())
        {
            return; // DB is already seeded
        }

        var notes = new Note[]
        {
            new Note { Title = "Example Title 1", Body = "Example Body 1" },
            new Note { Title = "Example Title 2", Body = "Example Body 2" },
            new Note { Title = "Example Title 3", Body = "Example Body 3" },
            new Note { Title = "Example Title 4", Body = "Example Body 4" },
        };
        
        context.Notes.AddRange(notes);
        context.SaveChanges();

        var users = new User[]
        {
            new User
            {
                Email = "Example Email 1", FirstName = "FirstName1", LastName = "LName1", Username = "Username1", PasswordHash = "1234"
            },
            new User
            {
                Email = "Example Email 2", FirstName = "FirstName2", LastName = "LName2", Username = "Username2", PasswordHash = "4312"
            },
            new User
            {
                Email = "Example Email 4", FirstName = "FirstName4", LastName = "LName4", Username = "Username4", PasswordHash = "4341~"
            },
            new User
            {
                Email = "Example Email 3", FirstName = "FirstName3", LastName = "LName3", Username = "Username3", PasswordHash = "4121"
            },
        };

        context.Users.AddRange(users);
        context.SaveChanges();
    }
    
}