using Microsoft.EntityFrameworkCore;
using NoteApp.Models;

namespace NoteApp.Helpers;

public class NoteDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public NoteDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(Configuration.GetConnectionString("NoteDbContext"));
    }
    
    public DbSet<User> User { get; set; }
    public DbSet<Note> Note { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Note>().ToTable("Note");
    }
}