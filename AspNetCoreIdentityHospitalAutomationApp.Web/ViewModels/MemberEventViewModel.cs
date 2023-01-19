using AspNetCoreIdentityHospitalAutomationApp.Web.Enums;
using AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace HospitalAutomationApp.Web.ViewModels
{
    public class MemberEventViewModel 
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public Branches Branch { get; set; }

        public DateTime SelectedDate { get; set; }

        
    }
}
