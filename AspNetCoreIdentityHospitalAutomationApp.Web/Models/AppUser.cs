using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Models
{
    public class AppUser:IdentityUser
    {
        public string? City { get; set; }

        public string? Picture { get; set; }

        public DateTime? BirthDay { get; set; }

        public int Gender { get; set; }

        public int Branch { get; set; }
    }
}
