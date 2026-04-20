using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using TimemanagementPlatform.Web.Data;
using TimemanagementPlatform.Web.Models;
using TimemanagementPlatform.Web.Services.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IEmailSender, TimemanagementPlatform.Web.Services.Identity.NoOpEmailSender>();

var app = builder.Build();

await InitializeDatabaseAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (!app.Configuration.GetValue<bool>("DisableHttpsRedirect"))
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

static async Task InitializeDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    if (app.Configuration.GetValue<bool>("Database:EnsureCreated", true))
    {
        await db.Database.EnsureCreatedAsync();
    }
    else if (app.Configuration.GetValue<bool>("Database:AutoMigrate", false))
    {
        await db.Database.MigrateAsync();
    }

    if (!app.Configuration.GetValue<bool>("Demo:SeedEnabled", false))
        return;

    var email = app.Configuration["DEMO_EMAIL"] ?? app.Configuration["Demo:Email"];
    var password = app.Configuration["DEMO_PASSWORD"] ?? app.Configuration["Demo:Password"];
    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        return;

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var existing = await userManager.FindByEmailAsync(email);
    if (existing == null)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new InvalidOperationException("Failed to seed demo user: " + string.Join("; ", result.Errors.Select(e => e.Description)));

        existing = user;
    }

    await SeedPortfolioDataAsync(db, existing!.Id);
}

static async Task SeedPortfolioDataAsync(ApplicationDbContext db, string userId)
{
    if (await db.Semesters.AnyAsync())
        return;

    var semester = new Semester
    {
        SemesterName = "Career Accelerator Semester",
        SemesterWeeks = 14,
        StartDate = DateTime.UtcNow.Date.AddDays(-14),
        UserId = userId
    };

    db.Semesters.Add(semester);
    await db.SaveChangesAsync();

    var module = new Module
    {
        Code = "PORT-401",
        Name = "Portfolio Project Engineering",
        Credits = 16,
        ClassHrsWeekly = 4,
        HrsSelfStudy = 12,
        HrsSelfStudyLeft = 9,
        SelectedColourHex = "#1f6feb",
        SelectedColourName = "Azure",
        SemesterId = semester.SemesterId,
        UserId = userId
    };

    db.Modules.Add(module);
    await db.SaveChangesAsync();

    db.ModuleHours.AddRange(
        new ModuleHour
        {
            ModuleId = module.ModuleId,
            SemesterId = semester.SemesterId,
            UserId = userId,
            Date = DateTime.UtcNow.Date.AddDays(-7),
            Hours = 2
        },
        new ModuleHour
        {
            ModuleId = module.ModuleId,
            SemesterId = semester.SemesterId,
            UserId = userId,
            Date = DateTime.UtcNow.Date.AddDays(-3),
            Hours = 3
        });

    await db.SaveChangesAsync();
}
