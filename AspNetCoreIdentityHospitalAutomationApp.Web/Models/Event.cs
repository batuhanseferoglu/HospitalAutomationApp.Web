using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Models
{
    public class Event
    {
        public int EventId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? StartEvent { get; set; }

        public string? EndEvent { get; set; }

        public bool AllDay { get; set; }
    }
}
        
