using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoteApp.Models;

namespace NoteApp.Helpers;

public class NoteDbContext : IdentityDbContext<User>
{
    protected readonly IConfiguration Configuration;

    public NoteDbContext(IConfiguration configuration, DbContextOptions<NoteDbContext> options)
        : base(options)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(Configuration.GetConnectionString("NoteDbContext"));
    }
    
    public DbSet<Note> Notes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Note>().ToTable("Note");
    }
}

public class UserManagementContext : IdentityDbContext<User>
{
    
    protected readonly IConfiguration Configuration;

    public UserManagementContext(IConfiguration configuration, DbContextOptions<UserManagementContext> options)
        : base(options)
    {
        Configuration = configuration;
    }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(Configuration.GetConnectionString("NoteDbContext"));
    }    
    
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("User");
    }

}