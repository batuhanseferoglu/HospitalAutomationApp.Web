using AspNetCoreIdentityHospitalAutomationApp.Web.DataAccessLayer;
using AspNetCoreIdentityHospitalAutomationApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Globalization;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Controllers
{
    public class CalendarController : Controller
    {
        private DA _DA { get; set; }

        public CalendarController(IOptions<AppSettings> settings)
        {
            _DA = new DA(settings.Value.DefaultAzureConnectionString);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetCalendarEvents(string start, string end)
        {
            List<Event> events = _DA.GetCalendarEvents(start, end);

           

            return Json(events);
        }

        [HttpGet]
        public IActionResult ListEvents(string start, string end)
        {
            List<Event> events = _DA.GetCalendarEvents(start,end);
            return View(events);
        }


        [HttpPost]
        public IActionResult UpdateEvent([FromBody] Event evt)
        {
            string message = String.Empty;

            message = _DA.UpdateEvent(evt);

            return Json(new { message });
        }

        [HttpPost]
        public IActionResult AddEvent([FromBody] Event evt)
        {
            

            string message = String.Empty;
            int eventId = 0;

            message = _DA.AddEvent(evt, out eventId);

            return Json(new { message, eventId });
        }

        [HttpPost]
        public IActionResult DeleteEvent([FromBody] Event evt)
        {
            string message = String.Empty;

            message = _DA.DeleteEvent(evt.EventId);

            return Json(new { message });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
