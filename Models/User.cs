namespace NoteApp.Models;

public class User
{
    private Guid _id = Guid.NewGuid();
    private string _email = null!;
    private string _username = null!;
    private string firstName = null!;
    private string _lastName = null!;
    private string _passwordHash = null!;
}