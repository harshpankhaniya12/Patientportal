using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Patientportal.AllApicall;
using Patientportal.Model;

namespace Patientportal.Pages.Calendar
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        //private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;
        public List<AppointmentListItem> Doctorblocktime { get; set; } = new List<AppointmentListItem>();
   

        public string? EjsDateTimePattern = "dd/MM/yyyy hh:mm:ss a";
        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClientFactory, ApiService apiService)
        {
            _logger = logger;
            _httpClient = httpClientFactory;
            _apiService = apiService;
        }
        public async Task OnGet()
        {
            string apiUrl2 = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/GetAppointmentsByDoctor";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiYjk5MDM2ZWItNTRhZS00ZWE0LWI1MjMtNThmYThlM2UzMzdkIiwibmJmIjoxNzQwNTU2NDQ1LCJleHAiOjE3NzIwOTI0NDUsImlhdCI6MTc0MDU1NjQ0NSwiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.yex9R3CP67Mkp715Y61FEIUIFhtiQhGJa8X01V_vEd_c9PuKw4uZbEi3_bQtpzQpwukb5uS_SPi4TN2HGh_JBQ"; // Valid token yahan dalein

            Doctorblocktime = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl2, token) ?? new List<AppointmentListItem>();
            if (Doctorblocktime != null && Doctorblocktime.Count > 0)
            {
                foreach (var appointment in Doctorblocktime)
                {
                    if (appointment.AppointmentStartTime.HasValue)
                    {
                        appointment.AppointmentStartTime = appointment.AppointmentStartTime.Value.AddHours(-5).AddMinutes(-30);
                        appointment.AppointmentEndDateTime = appointment.AppointmentEndDateTime.Value.AddHours(-5).AddMinutes(-30);
                    }
                }
            }
        }
        public async Task OnGetCalendarAsync()
        {
            string apiUrl2 = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/GetAppointmentsByDoctor";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiYjk5MDM2ZWItNTRhZS00ZWE0LWI1MjMtNThmYThlM2UzMzdkIiwibmJmIjoxNzQwNTU2NDQ1LCJleHAiOjE3NzIwOTI0NDUsImlhdCI6MTc0MDU1NjQ0NSwiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.yex9R3CP67Mkp715Y61FEIUIFhtiQhGJa8X01V_vEd_c9PuKw4uZbEi3_bQtpzQpwukb5uS_SPi4TN2HGh_JBQ"; // Valid token yahan dalein

            Doctorblocktime = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl2, token) ?? new List<AppointmentListItem>();
            if (Doctorblocktime != null && Doctorblocktime.Count > 0)
            {
                foreach (var appointment in Doctorblocktime)
                {
                    if (appointment.AppointmentStartTime.HasValue)
                    {
                        appointment.AppointmentStartTime = appointment.AppointmentStartTime.Value.AddHours(-5).AddMinutes(-30);
                        appointment.AppointmentEndDateTime = appointment.AppointmentEndDateTime.Value.AddHours(-5).AddMinutes(-30);
                    }
                }
            }
        }
    }
}
