using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı alanı boş bırakılamaz!")]
        [Display(Name = "Kullanıcı Adı:")]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email formatı hatalı!")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz!")]
        [Display(Name = "Email:")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz!")]
        [Display(Name = "Telefon:")]
        [RegularExpression("^(0(\\d{3}) (\\d{3}) (\\d{2}) (\\d{2}))$", ErrorMessage = "Telefon numarası uygun formatta değil!")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
        [Display(Name = "Şifre:")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Şifreler aynı değil!")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz!")]
        [Display(Name = "Şifre Tekrar:")]
        [DataType(DataType.Password)]
        public string? PasswordConfirm { get; set; }
    }
}

