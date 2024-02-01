using Microsoft.AspNetCore.Authorization;
using WebApp_UnderTheHood.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add the AddAuthentication method to the ConfigureServices method.

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", config =>
    {
        config.Cookie.Name = "MyCookieAuth"; // the name of the cookie
        config.LoginPath = "/Account/Login"; // the login page path
        config.AccessDeniedPath = "/Account/AccessDenied"; // the access denied page path
        config.LogoutPath = "/Account/Logout"; // the logout page path
        config.ExpireTimeSpan = TimeSpan.FromSeconds(30); // the cookie will expire after 5 minutes
    });

// Add the AddAuthorization method to the ConfigureServices method.

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("MustBeHR", policy => policy.RequireClaim("Department", "HR"));
    config.AddPolicy("MustBeAdmin", policy => policy.RequireClaim("Admin"));
    config.AddPolicy("MustBeHRManager", policy => policy
                                    .RequireClaim("Department", "HR")
                                    .RequireClaim("Manager")
                                    .Requirements.Add(new HRManagerProbationRequirement(3))); // add a custom requirement
});

// add a dependency injection for the custom requirement handler through the AddSingleton method
builder.Services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// start of the application middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
