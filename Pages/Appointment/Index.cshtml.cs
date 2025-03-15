using Innovura.CSharp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Patientportal.AllApicall;
using Patientportal.Model;
using Syncfusion.EJ2.Base;

namespace Patientportal.Pages.Appointment
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        //private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;
        public AppointmentListItem AppoinmentData { get; set; }
        public List<Holidays> Holidays { get; set; } = new List<Holidays>();
        [FromQuery(Name = "selectedDateTime")]
        public DateTimeOffset SelectedDateTime { get; set; }
        public string? EjsDateTimePattern = "dd/MM/yyyy hh:mm:ss a";
        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClientFactory, ApiService apiService)
        {
            _logger = logger;
            _httpClient = httpClientFactory;
            _apiService = apiService;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostPirescheduleAsync()
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var json = await reader.ReadToEndAsync();
            AppointmentListItem viewModel = JSON.Deserialize<AppointmentListItem>(json);


            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/AddAppointmentbyPatientPortal";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiYjk5MDM2ZWItNTRhZS00ZWE0LWI1MjMtNThmYThlM2UzMzdkIiwibmJmIjoxNzQwNTU2NDQ1LCJleHAiOjE3NzIwOTI0NDUsImlhdCI6MTc0MDU1NjQ0NSwiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.yex9R3CP67Mkp715Y61FEIUIFhtiQhGJa8X01V_vEd_c9PuKw4uZbEi3_bQtpzQpwukb5uS_SPi4TN2HGh_JBQ"; // Valid token yahan dalein
            string apiUrl5 = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Holiday/getHolidaysList";

            Holidays = await _apiService.GetAsync<List<Holidays>>(apiUrl5, token) ?? new List<Holidays>();
            var appointmentDate = viewModel.AppointmentStartTime.Value.ToString("dd/MM/yyyy");
            var isHoliday = Holidays.Any(h => h.StartDate.HasValue &&
                                              h.StartDate.Value.ToString("dd/MM/yyyy") == appointmentDate);

            if (isHoliday)
            {
                return new JsonResult(new { isSuccess = false, errorMessage = "Appointments cannot be scheduled on holidays." });
            }
            var apiHelper = new ApiService(_httpClient);
            var response = await _apiService.PostAsync<AppointmentListItem, ApiResponse>(apiUrl, viewModel, token);

            if (response != null && response.IsSuccess)
            {
                return new JsonResult(new { isSuccess = true, message = "Your change request has been submitted." });

            }
            else
            {
                return BadRequest("Failed to save patient details.");
            }
        }
    }
}
