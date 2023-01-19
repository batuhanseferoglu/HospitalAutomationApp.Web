using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels
{
    public class MedicineViewModel
    {
        public int Id { get; set; }

        [Display(Name = "İlaç Adı")]
        public string? MedName { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Ücret")]
        public decimal Price { get; set; }

        
    }
}
