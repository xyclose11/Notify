using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoteApp.Models;

namespace NoteApp.Helpers;

public class NoteDbContext : DbContext
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
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<NoteTag> NoteTags { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<NoteTag>().HasKey(sc => new { sc.NoteId, sc.TagId });
        modelBuilder.Entity<Note>().ToTable("Note");
        modelBuilder.Entity<Category>().ToTable("Category");
        modelBuilder.Entity<Tag>().ToTable("Tag");
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
    }

}