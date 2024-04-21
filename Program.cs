using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using test1app.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=MyDb.db")
);
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Ensure the session cookie is not accessible via JavaScript
    options.Cookie.IsEssential = true; // Make the session cookie essential to the application's operation
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Enable session middleware
app.UseSession();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
