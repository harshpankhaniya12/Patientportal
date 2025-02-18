using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Syncfusion.EJ2.Base;

namespace Patientportal.Pages
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public JsonResult OnPostAppointmentView([FromBody] DataManagerRequest dm)
        {
            var appointments = new List<object>
                {
                    new { AppointmentType = "Consultation", DoctorName = "Dr. Sejal Saheta", FormOfAppointment = "In-Person", AppointmentDateTime = DateTime.Now.AddHours(2), Status = "Confirmed" },
                    new { AppointmentType = "Follow-up", DoctorName = "Dr. Ramesh Patel", FormOfAppointment = "Online", AppointmentDateTime = DateTime.Now.AddDays(1), Status = "Pending" },
                    new { AppointmentType = "Routine Checkup", DoctorName = "Dr. Anita Sharma", FormOfAppointment = "In-Person", AppointmentDateTime = DateTime.Now.AddDays(3), Status = "Completed" }
                };

            var dataCount = appointments.Count;

            return new JsonResult(new { result = appointments, count = dataCount });
        }

        public void OnGet()
        {

        }
    }
}
