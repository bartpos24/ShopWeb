using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Extensions;
using ShopWeb.Infrastructure;
using ShopWeb.Infrastructure.Extensions;
using ShopWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//Infrastructure configuration
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
        //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        //options.ExpireTimeSpan = TimeSpan.FromHours(8);
        //options.SlidingExpiration = true;
        //options.Cookie.Name = "ShopWeb.Auth";
        //options.Cookie.HttpOnly = true;
        //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //options.Cookie.SameSite = SameSiteMode.Lax;
    });

builder.Services.AddAuthorization();

// Add NSwag Infrastructure services with authentication
builder.Services.AddInfrastructureServicesWithAuth(builder.Configuration);
// Add OpenAPI generated clients
builder.Services.AddOpenApiGeneratedClientsWithAuth(builder.Configuration);
//Application services
builder.Services.AddApplication();
//Infrastructure services
builder.Services.AddInfrastructure();

// Register background service for token monitoring
builder.Services.AddHostedService<TokenMonitorBackgroundService>();
var app = builder.Build();

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

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

// Add token refresh middleware
app.UseMiddleware<TokenRefreshMiddleware>();
app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
