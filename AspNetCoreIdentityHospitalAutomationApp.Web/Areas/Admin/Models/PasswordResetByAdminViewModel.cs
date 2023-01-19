using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Areas.Admin.Models
{
    public class PasswordResetByAdminViewModel
    {
        public string? UserId { get; set; }

        [Display(Name = "Yeni Şifre")]
        public string? NewPassword { get; set; }
    }
}
