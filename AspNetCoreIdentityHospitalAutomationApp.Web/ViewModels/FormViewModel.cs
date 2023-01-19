using AspNetCoreIdentityHospitalAutomationApp.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels
{
    public class FormViewModel 
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad/Soyad alanı boş bırakılamaz!")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Email formatı hatalı!")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz!")]
        public string EmailAddress { get; set; }

        public DateTime Date { get; set; }

        public string SelectedBranch { get; set; }

        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz!")]
        [DataType(DataType.PhoneNumber)]
        public string Number { get; set; }
        
        [MaxLength(500)]
        public string Message { get; set; }
    }
}

        

