using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using StudentSearch.Data;
using StudentSearch.Data.SeedData;
using StudentSearch.Models;
using StudentSearch.Repository;
using StudentSearch.Repository.Interfaces;
using StudentSearch.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StudentSearchContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentSearchContext") ?? throw new InvalidOperationException("Connection string 'StudentSearchContext' not found.")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<StudentSearchContext>();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedDatabase.Initialize(services);
}

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
