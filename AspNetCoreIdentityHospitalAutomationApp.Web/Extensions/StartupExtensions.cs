using AspNetCoreIdentityHospitalAutomationApp.Web.CustomValidations;
using AspNetCoreIdentityHospitalAutomationApp.Web.Localizations;
using AspNetCoreIdentityHospitalAutomationApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Extensions
{
    public static class StartupExtensions
    {
        public static void AddIdentityExtension(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcçdefgğhıijklmnoöpqrsştuüvwxyzABCÇDEFGHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._";
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 3;

            }).AddPasswordValidator<PasswordValidator>()
            .AddUserValidator<UserValidator>()
            .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<HospitalAppDbContext>();
        }
    }
}
