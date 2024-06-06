using Microsoft.AspNetCore.Identity;
using NoteApp.Helpers;

namespace NoteApp.Models.Helpers;

public static class DbInit
{
    public static async Task Initialize(NoteDbContext noteContext, UserManagementContext userContext, UserManager<User> userManager)
    {
        // if (noteContext.Notes.Any())
        // {
        //     return; // DB is already seeded
        // }
        //
        // var notes = new Note[]
        // {
        //     new Note { Title = "Example Title 1", Body = "Example Body 1" },
        //     new Note { Title = "Example Title 2", Body = "Example Body 2" },
        //     new Note { Title = "Example Title 3", Body = "Example Body 3" },
        //     new Note { Title = "Example Title 4", Body = "Example Body 4" },
        // };
        //
        // noteContext.Notes.AddRange(notes);
        // await noteContext.SaveChangesAsync();

        if (userContext.Users.Any())
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.WriteLine("User DB is Already seeded");
            Console.ResetColor();
            return; // User database is already seeded
        }

        var users = new User[]
        {
            new User
            {
                ConcurrencyStamp = "1", Email = "thisis@example.com", FirstName = "FirstName1", LastName = "LName1",LockoutEnabled = false,NormalizedEmail = "thisis@example.com",NormalizedUserName = "Username1",
                PasswordHash = "g0valp0", PhoneNumber = "1234567890", PhoneNumberConfirmed = false, SecurityStamp = "1234567890", TwoFactorEnabled = false, UserName = "Username1", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now
            },

        };

        foreach (var user in users)
        {
            var result = await userManager.CreateAsync(user, "DefaultPassword123!");
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.WriteLine(result);
            Console.ResetColor();
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("\n", result.Errors.Select(x => x.Description)));
            }
        }
    }
}