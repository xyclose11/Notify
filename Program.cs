using NoteApp.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NoteApp.Models;
using NoteApp.Models.Helpers;
using NoteApp.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Database Connection
builder.Services.AddDbContext<NoteDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("NoteDbContext")));

builder.Services.AddDbContext<UserManagementContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("NoteDbContext")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();



#endregion

#region UserAuthentication

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserManagementContext>();

builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password Settings.
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredUniqueChars = 2;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 6;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);

    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/UnauthorizedAccess";
    options.SlidingExpiration = true;
});
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var noteContext = services.GetRequiredService<NoteDbContext>();
    noteContext.Database.EnsureCreated();

    var userContext = services.GetRequiredService<UserManagementContext>();
    userContext.Database.EnsureCreated();
    
    var userManager = services.GetRequiredService<UserManager<User>>();
    
    DbInit.Initialize(noteContext, userContext, userManager);

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    if (!roleManager.RoleExistsAsync("Admin").Result)
    {
        var role = new IdentityRole("Admin");
        var roleResult = roleManager.CreateAsync(role).Result;
    }

    if (!roleManager.RoleExistsAsync("User").Result)
    {
        var role = new IdentityRole("User");
        var roleResult = roleManager.CreateAsync(role).Result;
    }

    if (!roleManager.RoleExistsAsync("Guest").Result)
    {
        var role = new IdentityRole("Guest");
        var roleResult = roleManager.CreateAsync(role).Result;
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();