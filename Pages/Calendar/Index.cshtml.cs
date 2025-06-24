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
        private readonly IConfiguration _configuration;
        public List<AppointmentListItem> Doctorblocktime { get; set; } = new List<AppointmentListItem>();
        public List<Holidays> Holidays { get; set; } = new List<Holidays>();


        public string? EjsDateTimePattern = "dd/MM/yyyy hh:mm:ss a";
        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClientFactory, ApiService apiService, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClientFactory;
            _apiService = apiService;
            _configuration = configuration;
        }
        public async Task OnGet()
        {
            string baseUrl = _configuration["ApiSettings:BaseUrl"];
            string token = _configuration["ApiSettings:AuthToken"];

            string apiUrl = $"{baseUrl}/api/v1/Holiday/getHolidaysList";
            string apiUrl2 = $"{baseUrl}/api/v1/Appointment/GetAppointmentsByDoctor";

            Doctorblocktime = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl2, token) ?? new List<AppointmentListItem>();
            Holidays = await _apiService.GetAsync<List<Holidays>>(apiUrl, token) ?? new List<Holidays>();
            //foreach (var appointment in Doctorblocktime)
            //{
            //    if (appointment.AppointmentStartTime != null)
            //    {
            //        appointment.AppointmentStartTime = appointment.AppointmentStartTime.Value.AddHours(-5).AddMinutes(-30);
            //    }

            //    if (appointment.AppointmentEndDateTime != null)
            //    {
            //        appointment.AppointmentEndDateTime = appointment.AppointmentEndDateTime.Value.AddHours(-5).AddMinutes(-30);
            //    }
            //}

        
        
        
        
        
        
        }
        public async Task OnGetCalendarAsync()
        {
            string baseUrl = _configuration["ApiSettings:BaseUrl"];
            string token = _configuration["ApiSettings:AuthToken"];

            string apiUrl2 = $"{baseUrl}/api/v1/Appointment/GetAppointmentsByDoctor";
            
            Doctorblocktime = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl2, token) ?? new List<AppointmentListItem>();
            //if (Doctorblocktime != null && Doctorblocktime.Count > 0)
            //{
            //    foreach (var appointment in Doctorblocktime)
            //    {
            //        if (appointment.AppointmentStartTime != null)
            //        {
            //            appointment.AppointmentStartTime = appointment.AppointmentStartTime.Value.AddHours(-5).AddMinutes(-30);
            //        }

            //        if (appointment.AppointmentEndDateTime != null)
            //        {
            //            appointment.AppointmentEndDateTime = appointment.AppointmentEndDateTime.Value.AddHours(-5).AddMinutes(-30);
            //        }
            //    }
            //}
        }
    }
}
