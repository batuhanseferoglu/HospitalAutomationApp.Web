using AspNetCoreIdentityHospitalAutomationApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels
{
    public class PatientFileViewModel
    {
        public int Id { get; set; } 

        [Display(Name = "Hasta Id:")]
        public string? PatientId { get; set; }

        [Display(Name = "Dosya Adı:")]
        public string? FileName { get; set; }

        [Display(Name = "Dosya Türü:")]
        public string? ContentType { get; set; }

        [Display(Name = "Boyut:")]
        public byte[]? Data { get; set; }

        //public virtual AppUser user { get; set; }
       
    }
}
