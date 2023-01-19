using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Yeni Şifre alanı boş bırakılamaz!")]
        [Display(Name = "Yeni Şifre:")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Şifreler aynı değil!")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz!")]
        [Display(Name = "Yeni Şifre Tekrar:")]
        [DataType(DataType.Password)]
        public string? PasswordConfirm { get; set; }
    }
}
