using AspNetCoreIdentityHospitalAutomationApp.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels
{
    public class MemberViewModel
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
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^(0(\\d{3}) (\\d{3}) (\\d{2}) (\\d{2}))$", ErrorMessage = "Telefon numarası uygun formatta değil!")]
        public string? PhoneNumber { get; set; }

       

        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }

        
        public string? Picture { get; set; }

        [Display(Name = "Cinsiyet")]
        public Gender Gender { get; set; }

        [Display(Name = "Şehir")]
        public string? City { get; set; }
    }
}
