using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Areas.Admin.Models
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Role ismi gereklidir!")]
        public string? Name { get; set; }

        public string? Id { get; set; }
    }
}
