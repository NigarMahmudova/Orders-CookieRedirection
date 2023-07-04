using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PustokBookStore.DAL;
using PustokBookStore.Entities;
using PustokBookStore.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PustokDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddScoped<LayoutService>();

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireNonAlphanumeric = false;

}).AddDefaultTokenProviders().AddEntityFrameworkStores<PustokDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToAccessDenied = options.Events.OnRedirectToLogin = context =>
    {
        var uri = new Uri(context.RedirectUri);

        if (context.HttpContext.Request.Path.Value.StartsWith("/manage"))
            context.Response.Redirect("/manage/account/login" + uri.Query);
        else
            context.Response.Redirect("/account/login" + uri.Query);

        return Task.CompletedTask;
    };
});

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.Run();
