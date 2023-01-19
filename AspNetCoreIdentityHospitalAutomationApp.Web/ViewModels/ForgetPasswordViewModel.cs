using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email alanı boş bırakılamaz!")]
        [EmailAddress(ErrorMessage = "Email formatı hatalı!")]
        [Display(Name = "Email:")]
        public string? Email { get; set; }

    }
}
