using AspNetCoreIdentityHospitalAutomationApp.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels
{
    public class DoctorViewModel : MemberViewModel
    {
        [Display(Name = "Branş:")]
        public Branches Branch { get; set; }
    }
}
