using AspNetCoreIdentityHospitalAutomationApp.Web.Extensions;
using AspNetCoreIdentityHospitalAutomationApp.Web.Interfaces;
using AspNetCoreIdentityHospitalAutomationApp.Web.Models;
using AspNetCoreIdentityHospitalAutomationApp.Web.OptionsModel;
using AspNetCoreIdentityHospitalAutomationApp.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<HospitalAppDbContext>(options =>
{
    //options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultAzureConnectionString"));
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = configuration["Authentication:Facebook:AppId"];
    options.AppSecret = configuration["Authentication:Facebook:AppSecret"];
}).AddGoogle(options =>
{
    options.ClientId = configuration["Authentication:Google:ClientId"];
    options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
}).AddMicrosoftAccount(options =>
{
    options.ClientId = configuration["Authentication:Microsoft:ClientId"];
    options.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
});

builder.Services.AddIdentityExtension();

builder.Services.AddScoped<IEmailService,EmailService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    var cookieBuilder = new CookieBuilder();

    cookieBuilder.Name = "HospitalAppCookie";

    options.LoginPath = new PathString("/Home/SignIn");

    options.LogoutPath = new PathString("/Member/LogOut");

    options.Cookie = cookieBuilder;

    options.ExpireTimeSpan = TimeSpan.FromDays(60);

    options.SlidingExpiration = true;


});

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


